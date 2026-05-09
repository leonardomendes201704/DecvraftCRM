using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Application.Modules;
using Platform.Persistence;

namespace Platform.Infrastructure.Modules;

public sealed class ModuleCatalogService : IModuleCatalogService
{
    private readonly AppDbContext _dbContext;

    public ModuleCatalogService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<ModuleResponse>> GetModulesAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        var tenantModules = await _dbContext.TenantModules
            .Where(tenantModule => tenantModule.TenantId == tenantId)
            .ToDictionaryAsync(tenantModule => tenantModule.ModuleId, cancellationToken);

        var modules = await _dbContext.Modules
            .OrderBy(module => module.IsCore ? 0 : 1)
            .ThenBy(module => module.Name)
            .ToListAsync(cancellationToken);

        return modules
            .Select(module =>
            {
                var isInstalled = tenantModules.TryGetValue(module.Id, out var tenantModule);
                var isActiveForTenant = module.IsEnabled && isInstalled && tenantModule!.IsEnabled;

                return new ModuleResponse(
                    module.Id,
                    module.Name,
                    module.Slug,
                    module.Version,
                    module.IsCore,
                    module.IsEnabled,
                    isInstalled,
                    isActiveForTenant);
            })
            .ToArray();
    }
}
