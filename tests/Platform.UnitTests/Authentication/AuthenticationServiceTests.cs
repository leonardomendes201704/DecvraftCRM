using System.Globalization;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Platform.Application.Auth;
using Platform.Domain.Catalog;
using Platform.Domain.Entities;
using Platform.Domain.Security;
using Platform.Infrastructure.Auth;
using Platform.Persistence;

namespace Platform.UnitTests.Authentication;

public sealed class AuthenticationServiceTests
{
    [Fact]
    public async Task LoginAsync_returns_token_and_current_user_when_credentials_are_valid()
    {
        await using var dbContext = CreateDbContext();
        var seededData = await SeedTenantUserAndSecurityAsync(dbContext);
        var jwtTokenService = new JwtTokenService(dbContext);
        var service = new AuthenticationService(dbContext, new PasswordHashVerifier(), jwtTokenService);

        var result = await service.LoginAsync(new LoginRequest("acme", "admin@acme.test", "StrongPassword123!"));

        Assert.True(result.Succeeded);
        Assert.NotNull(result.Login);
        Assert.False(string.IsNullOrWhiteSpace(result.Login.AccessToken));
        Assert.Equal(seededData.UserId, result.Login.User.UserId);
        Assert.Equal(seededData.TenantId, result.Login.User.TenantId);
        Assert.Contains(KnownRoles.TenantAdmin, result.Login.User.Roles);
        Assert.Contains("crm.customers.read", result.Login.User.Permissions);
    }

    [Fact]
    public async Task GetCurrentUserAsync_returns_user_from_valid_token()
    {
        await using var dbContext = CreateDbContext();
        var seededData = await SeedTenantUserAndSecurityAsync(dbContext);
        var jwtTokenService = new JwtTokenService(dbContext);
        var service = new AuthenticationService(dbContext, new PasswordHashVerifier(), jwtTokenService);
        var login = await service.LoginAsync(new LoginRequest("acme", "admin@acme.test", "StrongPassword123!"));

        var currentUser = await service.GetCurrentUserAsync(login.Login!.AccessToken);

        Assert.NotNull(currentUser);
        Assert.Equal(seededData.UserId, currentUser.UserId);
        Assert.Equal("acme", currentUser.TenantSlug);
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static async Task<SeededData> SeedTenantUserAndSecurityAsync(AppDbContext dbContext)
    {
        var now = DateTimeOffset.UtcNow;
        var tenant = Tenant.Create("Acme", "acme", customDomain: null, now);
        var user = ApplicationUser.Create(
            tenant.Id,
            "Acme Admin",
            "admin@acme.test",
            CreatePasswordHash("StrongPassword123!"),
            now);
        var role = Role.Create(tenant.Id, KnownRoles.TenantAdmin, isSystemRole: true);
        var permission = Permission.Create("crm.customers.read", "Read CRM customers", KnownModules.CrmSlug);

        dbContext.Tenants.Add(tenant);
        dbContext.ApplicationUsers.Add(user);
        dbContext.Roles.Add(role);
        dbContext.Permissions.Add(permission);
        dbContext.UserRoles.Add(UserRole.Create(tenant.Id, user.Id, role.Id));
        dbContext.RolePermissions.Add(RolePermission.Create(tenant.Id, role.Id, permission.Id));
        dbContext.SystemConfigurations.Add(SystemConfiguration.Create(
            KnownSystemConfigurationKeys.JwtSecret,
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(JwtConfigurationDefaults.SecretSizeInBytes)),
            isSecret: true,
            now));
        dbContext.SystemConfigurations.Add(SystemConfiguration.Create(
            KnownSystemConfigurationKeys.JwtIssuer,
            JwtConfigurationDefaults.Issuer,
            isSecret: false,
            now));
        dbContext.SystemConfigurations.Add(SystemConfiguration.Create(
            KnownSystemConfigurationKeys.JwtAudience,
            JwtConfigurationDefaults.Audience,
            isSecret: false,
            now));
        dbContext.SystemConfigurations.Add(SystemConfiguration.Create(
            KnownSystemConfigurationKeys.JwtAccessTokenMinutes,
            JwtConfigurationDefaults.AccessTokenMinutes.ToString(CultureInfo.InvariantCulture),
            isSecret: false,
            now));

        await dbContext.SaveChangesAsync();

        return new SeededData(tenant.Id, user.Id);
    }

    private static string CreatePasswordHash(string password)
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
            PasswordHashingDefaults.Iterations.ToString(CultureInfo.InvariantCulture),
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash));
    }

    private sealed record SeededData(Guid TenantId, Guid UserId);
}
