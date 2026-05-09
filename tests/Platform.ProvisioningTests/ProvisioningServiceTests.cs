using Microsoft.EntityFrameworkCore;
using Platform.Domain.Catalog;
using Platform.Domain.Security;
using Platform.Persistence;
using Platform.Provisioning.Models;
using Platform.Provisioning.Services;
using Platform.ProvisioningTests.Support;

namespace Platform.ProvisioningTests;

public sealed class ProvisioningServiceTests
{
    private const string CompanyName = "Empresa Demo";
    private const string TenantSlug = "empresa-demo";
    private const string SystemName = "ERP Demo";
    private const string AdminName = "Administrador";
    private const string AdminEmail = "admin@demo.com";
    private const string AdminPassword = "Admin@123456";

    [Fact]
    public async Task SeedRunner_ShouldCreateKnownModulesAndPermissionsIdempotently()
    {
        await using var dbContext = ProvisioningTestDbContextFactory.Create();
        var seedRunner = new SeedRunner(dbContext);

        await seedRunner.RunAsync();
        await seedRunner.RunAsync();

        Assert.Equal(KnownModules.All.Count, await dbContext.Modules.CountAsync());
        Assert.Equal(KnownPermissions.All.Count, await dbContext.Permissions.CountAsync());
    }

    [Fact]
    public async Task ModuleInstaller_ShouldInstallRequestedModulesOnlyOnce()
    {
        await using var dbContext = ProvisioningTestDbContextFactory.Create();
        await new SeedRunner(dbContext).RunAsync();

        var tenantId = await CreateTenantAsync(dbContext);
        var moduleInstaller = new ModuleInstaller(dbContext);

        await moduleInstaller.InstallBaseModulesAsync(tenantId, KnownModules.DefaultSlugs);
        await moduleInstaller.InstallBaseModulesAsync(tenantId, KnownModules.DefaultSlugs);

        Assert.Equal(KnownModules.DefaultSlugs.Count, await dbContext.TenantModules.CountAsync());
    }

    [Fact]
    public async Task InstallerLockService_ShouldLockInstallationAndExposeStatus()
    {
        await using var dbContext = ProvisioningTestDbContextFactory.Create();
        var lockService = new InstallerLockService(dbContext);

        var initialStatus = await lockService.GetStatusAsync();
        await lockService.LockAsync(InstallerDefaults.Version, InstallerDefaults.InstalledBy);
        var lockedStatus = await lockService.GetStatusAsync();

        Assert.False(initialStatus.IsInstalled);
        Assert.True(lockedStatus.IsInstalled);
        Assert.Equal(InstallerDefaults.Version, lockedStatus.Version);
    }

    [Fact]
    public async Task TenantProvisioner_ShouldCreateTenantBrandingAdminAndTenantAdminRole()
    {
        await using var dbContext = ProvisioningTestDbContextFactory.Create();
        await new SeedRunner(dbContext).RunAsync();

        var tenantProvisioner = new TenantProvisioner(dbContext);

        var tenantId = await tenantProvisioner.CreateTenantAsync(new TenantSetupOptions
        {
            CompanyName = CompanyName,
            Slug = TenantSlug
        });

        var brandingId = await tenantProvisioner.CreateBrandingAsync(tenantId, new BrandingSetupOptions
        {
            SystemName = SystemName
        });

        var adminId = await tenantProvisioner.CreateAdminUserAsync(tenantId, new AdminUserSetupOptions
        {
            Name = AdminName,
            Email = AdminEmail,
            Password = AdminPassword
        });

        var admin = await dbContext.ApplicationUsers.SingleAsync(user => user.Id == adminId);
        var tenantAdminRole = await dbContext.Roles.SingleAsync(role => role.TenantId == tenantId && role.Name == KnownRoles.TenantAdmin);

        Assert.NotEqual(Guid.Empty, tenantId);
        Assert.NotEqual(Guid.Empty, brandingId);
        Assert.NotEqual(Guid.Empty, adminId);
        Assert.NotEqual(AdminPassword, admin.PasswordHash);
        Assert.Contains(PasswordHashingDefaults.Algorithm, admin.PasswordHash);
        Assert.True(await dbContext.UserRoles.AnyAsync(userRole => userRole.UserId == adminId && userRole.RoleId == tenantAdminRole.Id));
        Assert.Equal(KnownPermissions.All.Count, await dbContext.RolePermissions.CountAsync(rolePermission => rolePermission.RoleId == tenantAdminRole.Id));
    }

    private static async Task<Guid> CreateTenantAsync(AppDbContext dbContext)
    {
        var tenantProvisioner = new TenantProvisioner(dbContext);

        return await tenantProvisioner.CreateTenantAsync(new TenantSetupOptions
        {
            CompanyName = CompanyName,
            Slug = TenantSlug
        });
    }
}
