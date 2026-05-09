using Microsoft.EntityFrameworkCore;
using Platform.Persistence;
using Platform.Provisioning.Abstractions;

namespace Platform.Provisioning.Services;

public sealed class ModuleInstaller : IModuleInstaller
{
    private readonly AppDbContext _dbContext;

    public ModuleInstaller(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InstallBaseModulesAsync(Guid tenantId, IReadOnlyCollection<string> moduleSlugs, CancellationToken cancellationToken = default)
    {
        if (moduleSlugs.Count == 0)
        {
            return;
        }

        var normalizedSlugs = moduleSlugs
            .Select(slug => slug.Trim().ToLowerInvariant())
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        var modules = await _dbContext.Modules
            .Where(module => normalizedSlugs.Contains(module.Slug))
            .ToListAsync(cancellationToken);

        var missingSlugs = normalizedSlugs
            .Except(modules.Select(module => module.Slug), StringComparer.Ordinal)
            .ToArray();

        if (missingSlugs.Length > 0)
        {
            throw new InvalidOperationException($"Modules not registered: {string.Join(", ", missingSlugs)}");
        }

        foreach (var module in modules)
        {
            var alreadyInstalled = await _dbContext.TenantModules
                .AnyAsync(tenantModule => tenantModule.TenantId == tenantId && tenantModule.ModuleId == module.Id, cancellationToken);

            if (alreadyInstalled)
            {
                continue;
            }

            _dbContext.TenantModules.Add(Platform.Domain.Entities.TenantModule.Install(tenantId, module.Id, DateTimeOffset.UtcNow));
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
