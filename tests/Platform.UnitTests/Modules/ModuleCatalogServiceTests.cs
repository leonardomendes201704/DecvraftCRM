using Microsoft.EntityFrameworkCore;
using Platform.Domain.Catalog;
using Platform.Domain.Entities;
using Platform.Infrastructure.Modules;
using Platform.Persistence;
using DomainModule = Platform.Domain.Entities.Module;

namespace Platform.UnitTests.Modules;

public sealed class ModuleCatalogServiceTests
{
    [Fact]
    public async Task GetModulesAsync_returns_modules_with_tenant_activation_flags()
    {
        await using var dbContext = CreateDbContext();
        var tenant = Tenant.Create("Acme", "acme", customDomain: null, DateTimeOffset.UtcNow);
        var coreModule = DomainModule.Create("Core", KnownModules.CoreSlug, "1.0.0", isCore: true);
        var crmModule = DomainModule.Create("CRM", KnownModules.CrmSlug, "1.0.0", isCore: false);
        var financeModule = DomainModule.Create("Financeiro", KnownModules.FinanceSlug, "1.0.0", isCore: false);

        dbContext.Tenants.Add(tenant);
        dbContext.Modules.AddRange(coreModule, crmModule, financeModule);
        dbContext.TenantModules.Add(TenantModule.Install(tenant.Id, coreModule.Id, DateTimeOffset.UtcNow));
        dbContext.TenantModules.Add(TenantModule.Install(tenant.Id, crmModule.Id, DateTimeOffset.UtcNow));
        await dbContext.SaveChangesAsync();

        var service = new ModuleCatalogService(dbContext);

        var modules = await service.GetModulesAsync(tenant.Id);

        var core = Assert.Single(modules, module => module.Slug == KnownModules.CoreSlug);
        var crm = Assert.Single(modules, module => module.Slug == KnownModules.CrmSlug);
        var finance = Assert.Single(modules, module => module.Slug == KnownModules.FinanceSlug);

        Assert.True(core.IsInstalled);
        Assert.True(core.IsActiveForTenant);
        Assert.True(crm.IsInstalled);
        Assert.True(crm.IsActiveForTenant);
        Assert.False(finance.IsInstalled);
        Assert.False(finance.IsActiveForTenant);
    }

    [Fact]
    public async Task GetModulesAsync_does_not_mix_tenant_module_activation_between_tenants()
    {
        await using var dbContext = CreateDbContext();
        var firstTenant = Tenant.Create("Acme", "acme", customDomain: null, DateTimeOffset.UtcNow);
        var secondTenant = Tenant.Create("Beta", "beta", customDomain: null, DateTimeOffset.UtcNow);
        var coreModule = DomainModule.Create("Core", KnownModules.CoreSlug, "1.0.0", isCore: true);

        dbContext.Tenants.AddRange(firstTenant, secondTenant);
        dbContext.Modules.Add(coreModule);
        dbContext.TenantModules.Add(TenantModule.Install(firstTenant.Id, coreModule.Id, DateTimeOffset.UtcNow));
        await dbContext.SaveChangesAsync();

        var service = new ModuleCatalogService(dbContext);

        var modules = await service.GetModulesAsync(secondTenant.Id);

        var core = Assert.Single(modules);
        Assert.False(core.IsInstalled);
        Assert.False(core.IsActiveForTenant);
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
