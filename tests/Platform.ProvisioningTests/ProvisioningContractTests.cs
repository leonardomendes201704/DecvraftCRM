using Platform.Domain.Catalog;
using Platform.Provisioning.Models;

namespace Platform.ProvisioningTests;

public sealed class ProvisioningContractTests
{
    private const string DatabaseHost = "mssql";
    private const string DatabaseName = "WhiteLabelErp";
    private const string DatabaseUser = "sa";
    private const string BootstrapPassword = "bootstrap-only";
    private const string CompanyName = "Empresa Demo";
    private const string TenantSlug = "empresa-demo";
    private const string AdminName = "Administrador";
    private const string AdminEmail = "admin@demo.com";
    private const string AdminPassword = "Admin@123456";
    private const string SystemName = "ERP Demo";

    [Fact]
    public void InstallRequest_ShouldComposeInitialProvisioningPayload()
    {
        var request = new InstallRequest
        {
            Database = new DatabaseSetupOptions
            {
                Host = DatabaseHost,
                Port = 1433,
                DatabaseName = DatabaseName,
                Username = DatabaseUser,
                Password = BootstrapPassword
            },
            Tenant = new TenantSetupOptions
            {
                CompanyName = CompanyName,
                Slug = TenantSlug
            },
            AdminUser = new AdminUserSetupOptions
            {
                Name = AdminName,
                Email = AdminEmail,
                Password = AdminPassword
            },
            Branding = new BrandingSetupOptions
            {
                SystemName = SystemName
            },
            Modules = [.. KnownModules.DefaultSlugs]
        };

        Assert.Equal(DatabaseHost, request.Database.Host);
        Assert.Equal(1433, request.Database.Port);
        Assert.Equal(TenantSlug, request.Tenant.Slug);
        Assert.Contains(KnownModules.CoreSlug, request.Modules);
        Assert.Contains(KnownModules.CrmSlug, request.Modules);
        Assert.Contains(KnownModules.FinanceSlug, request.Modules);
    }

    [Fact]
    public void ProvisioningResult_ShouldRepresentSuccessAndFailure()
    {
        var tenantId = Guid.NewGuid();
        var adminUserId = Guid.NewGuid();

        var success = ProvisioningResult.Success(tenantId, adminUserId, [KnownModules.CoreSlug]);
        var failure = ProvisioningResult.Failure(["invalid install request"]);

        Assert.True(success.Succeeded);
        Assert.Equal(tenantId, success.TenantId);
        Assert.Equal(adminUserId, success.AdminUserId);
        Assert.Contains(KnownModules.CoreSlug, success.InstalledModules);

        Assert.False(failure.Succeeded);
        Assert.Contains("invalid install request", failure.Errors);
    }
}
