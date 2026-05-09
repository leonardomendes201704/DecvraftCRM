using Microsoft.Extensions.DependencyInjection;
using Platform.Provisioning.Abstractions;
using Platform.Provisioning.Services;

namespace Platform.Provisioning;

public static class DependencyInjection
{
    public static IServiceCollection AddProvisioning(this IServiceCollection services)
    {
        services.AddScoped<IMigrationRunner, MigrationRunner>();
        services.AddScoped<ISeedRunner, SeedRunner>();
        services.AddScoped<ITenantProvisioner, TenantProvisioner>();
        services.AddScoped<IModuleInstaller, ModuleInstaller>();
        services.AddScoped<IInstallerLockService, InstallerLockService>();
        services.AddScoped<IProvisioningService, ProvisioningService>();

        return services;
    }
}
