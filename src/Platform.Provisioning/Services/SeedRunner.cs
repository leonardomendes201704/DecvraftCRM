using Microsoft.EntityFrameworkCore;
using Platform.Domain.Catalog;
using Platform.Domain.Entities;
using Platform.Persistence;
using Platform.Provisioning.Abstractions;
using DomainModule = Platform.Domain.Entities.Module;

namespace Platform.Provisioning.Services;

public sealed class SeedRunner : ISeedRunner
{
    private readonly AppDbContext _dbContext;

    public SeedRunner(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        await SeedModulesAsync(cancellationToken);
        await SeedPermissionsAsync(cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedModulesAsync(CancellationToken cancellationToken)
    {
        foreach (var moduleDefinition in KnownModules.All)
        {
            var exists = await _dbContext.Modules
                .AnyAsync(module => module.Slug == moduleDefinition.Slug, cancellationToken);

            if (exists)
            {
                continue;
            }

            _dbContext.Modules.Add(DomainModule.Create(
                moduleDefinition.Name,
                moduleDefinition.Slug,
                moduleDefinition.Version,
                moduleDefinition.IsCore));
        }
    }

    private async Task SeedPermissionsAsync(CancellationToken cancellationToken)
    {
        foreach (var permissionDefinition in KnownPermissions.All)
        {
            var exists = await _dbContext.Permissions
                .AnyAsync(permission => permission.Key == permissionDefinition.Key, cancellationToken);

            if (exists)
            {
                continue;
            }

            _dbContext.Permissions.Add(Permission.Create(
                permissionDefinition.Key,
                permissionDefinition.Description,
                permissionDefinition.ModuleSlug));
        }
    }
}
