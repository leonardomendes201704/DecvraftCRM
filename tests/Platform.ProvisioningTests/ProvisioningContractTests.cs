using Platform.Provisioning.Models;

namespace Platform.ProvisioningTests;

public sealed class ProvisioningContractTests
{
    [Fact]
    public void InstallRequest_ShouldComposeInitialProvisioningPayload()
    {
        var request = new InstallRequest
        {
            Database = new DatabaseSetupOptions
            {
                Host = "mssql",
                Port = 1433,
                DatabaseName = "WhiteLabelErp",
                Username = "sa",
                Password = "bootstrap-only"
            },
            Tenant = new TenantSetupOptions
            {
                CompanyName = "Empresa Demo",
                Slug = "empresa-demo"
            },
            AdminUser = new AdminUserSetupOptions
            {
                Name = "Administrador",
                Email = "admin@demo.com",
                Password = "Admin@123456"
            },
            Branding = new BrandingSetupOptions
            {
                SystemName = "ERP Demo"
            },
            Modules = ["core", "crm", "finance"]
        };

        Assert.Equal("mssql", request.Database.Host);
        Assert.Equal(1433, request.Database.Port);
        Assert.Equal("empresa-demo", request.Tenant.Slug);
        Assert.Contains("core", request.Modules);
        Assert.Contains("crm", request.Modules);
        Assert.Contains("finance", request.Modules);
    }

    [Fact]
    public void ProvisioningResult_ShouldRepresentSuccessAndFailure()
    {
        var tenantId = Guid.NewGuid();
        var adminUserId = Guid.NewGuid();

        var success = ProvisioningResult.Success(tenantId, adminUserId, ["core"]);
        var failure = ProvisioningResult.Failure(["invalid install request"]);

        Assert.True(success.Succeeded);
        Assert.Equal(tenantId, success.TenantId);
        Assert.Equal(adminUserId, success.AdminUserId);
        Assert.Contains("core", success.InstalledModules);

        Assert.False(failure.Succeeded);
        Assert.Contains("invalid install request", failure.Errors);
    }
}
