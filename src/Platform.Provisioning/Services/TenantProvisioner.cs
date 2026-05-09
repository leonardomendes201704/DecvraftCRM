using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Platform.Domain.Catalog;
using Platform.Domain.Entities;
using Platform.Domain.Security;
using Platform.Persistence;
using Platform.Provisioning.Abstractions;
using Platform.Provisioning.Models;

namespace Platform.Provisioning.Services;

public sealed class TenantProvisioner : ITenantProvisioner
{
    private readonly AppDbContext _dbContext;

    public TenantProvisioner(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateTenantAsync(TenantSetupOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(options.CompanyName);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Slug);

        var slug = options.Slug.Trim().ToLowerInvariant();

        var existingTenant = await _dbContext.Tenants
            .SingleOrDefaultAsync(tenant => tenant.Slug == slug, cancellationToken);

        if (existingTenant is not null)
        {
            return existingTenant.Id;
        }

        var tenant = Tenant.Create(options.CompanyName.Trim(), slug, options.CustomDomain, DateTimeOffset.UtcNow);
        _dbContext.Tenants.Add(tenant);

        await _dbContext.SaveChangesAsync(cancellationToken);
        await EnsureDefaultRolesAsync(tenant.Id, cancellationToken);

        return tenant.Id;
    }

    public async Task<Guid> CreateBrandingAsync(Guid tenantId, BrandingSetupOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(options.SystemName);

        var existingBranding = await _dbContext.TenantBrandings
            .SingleOrDefaultAsync(branding => branding.TenantId == tenantId, cancellationToken);

        if (existingBranding is not null)
        {
            return existingBranding.Id;
        }

        var branding = TenantBranding.Create(
            tenantId,
            options.SystemName.Trim(),
            options.PrimaryColor,
            options.SecondaryColor);

        _dbContext.TenantBrandings.Add(branding);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return branding.Id;
    }

    public async Task<Guid> CreateAdminUserAsync(Guid tenantId, AdminUserSetupOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Email);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Password);

        var email = options.Email.Trim().ToLowerInvariant();

        var existingUser = await _dbContext.ApplicationUsers
            .SingleOrDefaultAsync(user => user.TenantId == tenantId && user.Email == email, cancellationToken);

        if (existingUser is not null)
        {
            await EnsureTenantAdminAssignmentAsync(tenantId, existingUser.Id, cancellationToken);
            return existingUser.Id;
        }

        var user = ApplicationUser.Create(
            tenantId,
            options.Name.Trim(),
            email,
            HashPassword(options.Password),
            DateTimeOffset.UtcNow);

        _dbContext.ApplicationUsers.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await EnsureTenantAdminAssignmentAsync(tenantId, user.Id, cancellationToken);

        return user.Id;
    }

    private async Task EnsureDefaultRolesAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        foreach (var roleName in KnownRoles.TenantDefaults)
        {
            var exists = await _dbContext.Roles
                .AnyAsync(role => role.TenantId == tenantId && role.Name == roleName, cancellationToken);

            if (exists)
            {
                continue;
            }

            _dbContext.Roles.Add(Role.Create(tenantId, roleName, isSystemRole: true));
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureTenantAdminAssignmentAsync(Guid tenantId, Guid userId, CancellationToken cancellationToken)
    {
        await EnsureDefaultRolesAsync(tenantId, cancellationToken);

        var tenantAdminRole = await _dbContext.Roles
            .SingleAsync(role => role.TenantId == tenantId && role.Name == KnownRoles.TenantAdmin, cancellationToken);

        var userRoleExists = await _dbContext.UserRoles
            .AnyAsync(userRole => userRole.TenantId == tenantId && userRole.UserId == userId && userRole.RoleId == tenantAdminRole.Id, cancellationToken);

        if (!userRoleExists)
        {
            _dbContext.UserRoles.Add(UserRole.Create(tenantId, userId, tenantAdminRole.Id));
        }

        var permissions = await _dbContext.Permissions.ToListAsync(cancellationToken);

        foreach (var permission in permissions)
        {
            var rolePermissionExists = await _dbContext.RolePermissions
                .AnyAsync(rolePermission =>
                    rolePermission.TenantId == tenantId &&
                    rolePermission.RoleId == tenantAdminRole.Id &&
                    rolePermission.PermissionId == permission.Id,
                    cancellationToken);

            if (rolePermissionExists)
            {
                continue;
            }

            _dbContext.RolePermissions.Add(RolePermission.Create(tenantId, tenantAdminRole.Id, permission.Id));
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(PasswordHashingDefaults.SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            PasswordHashingDefaults.Iterations,
            HashAlgorithmName.SHA256,
            PasswordHashingDefaults.HashSize);

        return string.Join(
            "$",
            PasswordHashingDefaults.Algorithm,
            PasswordHashingDefaults.Iterations.ToString(System.Globalization.CultureInfo.InvariantCulture),
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash));
    }
}
