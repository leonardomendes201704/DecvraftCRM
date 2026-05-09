using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Application.Auth;
using Platform.Domain.Catalog;
using Platform.Persistence;

namespace Platform.Infrastructure.Auth;

public sealed class AuthenticationService : IAuthenticationService
{
    private const string InvalidCredentialsError = "invalid_credentials";

    private readonly AppDbContext _dbContext;
    private readonly IPasswordHashVerifier _passwordHashVerifier;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthenticationService(
        AppDbContext dbContext,
        IPasswordHashVerifier passwordHashVerifier,
        IJwtTokenService jwtTokenService)
    {
        _dbContext = dbContext;
        _passwordHashVerifier = passwordHashVerifier;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthenticationResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.TenantSlug) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return AuthenticationResult.Failure(InvalidCredentialsError);
        }

        var tenantSlug = request.TenantSlug.Trim().ToLowerInvariant();
        var email = request.Email.Trim().ToLowerInvariant();

        var tenant = await _dbContext.Tenants
            .SingleOrDefaultAsync(item => item.Slug == tenantSlug && item.IsActive, cancellationToken);

        if (tenant is null)
        {
            return AuthenticationResult.Failure(InvalidCredentialsError);
        }

        var user = await _dbContext.ApplicationUsers
            .SingleOrDefaultAsync(item => item.TenantId == tenant.Id && item.Email == email && item.IsActive, cancellationToken);

        if (user is null || !_passwordHashVerifier.Verify(request.Password, user.PasswordHash))
        {
            return AuthenticationResult.Failure(InvalidCredentialsError);
        }

        var currentUser = await BuildCurrentUserAsync(tenant.Id, tenant.Slug, user.Id, user.Name, user.Email, cancellationToken);
        var loginResponse = await _jwtTokenService.CreateLoginResponseAsync(currentUser, cancellationToken);

        return AuthenticationResult.Success(loginResponse);
    }

    public async Task<CurrentUserResponse?> GetCurrentUserAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        var principal = await _jwtTokenService.ValidateAsync(accessToken, cancellationToken);
        if (principal is null)
        {
            return null;
        }

        var userIdValue = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var tenantIdValue = principal.FindFirst(KnownAuthClaimTypes.TenantId)?.Value;

        if (!Guid.TryParse(userIdValue, out var userId) || !Guid.TryParse(tenantIdValue, out var tenantId))
        {
            return null;
        }

        var tenant = await _dbContext.Tenants
            .SingleOrDefaultAsync(item => item.Id == tenantId && item.IsActive, cancellationToken);
        var user = await _dbContext.ApplicationUsers
            .SingleOrDefaultAsync(item => item.Id == userId && item.TenantId == tenantId && item.IsActive, cancellationToken);

        if (tenant is null || user is null)
        {
            return null;
        }

        return await BuildCurrentUserAsync(tenant.Id, tenant.Slug, user.Id, user.Name, user.Email, cancellationToken);
    }

    private async Task<CurrentUserResponse> BuildCurrentUserAsync(
        Guid tenantId,
        string tenantSlug,
        Guid userId,
        string name,
        string email,
        CancellationToken cancellationToken)
    {
        var roleIds = await _dbContext.UserRoles
            .Where(userRole => userRole.TenantId == tenantId && userRole.UserId == userId)
            .Select(userRole => userRole.RoleId)
            .ToListAsync(cancellationToken);

        var roles = await _dbContext.Roles
            .Where(role => role.TenantId == tenantId && roleIds.Contains(role.Id))
            .Select(role => role.Name)
            .OrderBy(role => role)
            .ToListAsync(cancellationToken);

        var permissions = await _dbContext.RolePermissions
            .Where(rolePermission => rolePermission.TenantId == tenantId && roleIds.Contains(rolePermission.RoleId))
            .Join(
                _dbContext.Permissions,
                rolePermission => rolePermission.PermissionId,
                permission => permission.Id,
                (_, permission) => permission.Key)
            .Distinct()
            .OrderBy(permission => permission)
            .ToListAsync(cancellationToken);

        return new CurrentUserResponse(userId, tenantId, tenantSlug, name, email, roles, permissions);
    }
}
