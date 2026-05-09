using Platform.Domain.Catalog;
using Platform.Provisioning.Abstractions;
using Platform.Provisioning.Models;
using Platform.Provisioning.Validation;

namespace Platform.Provisioning.Services;

public sealed class ProvisioningService : IProvisioningService
{
    private readonly IDatabaseProvisioner _databaseProvisioner;
    private readonly IMigrationRunner _migrationRunner;
    private readonly ISeedRunner _seedRunner;
    private readonly ITenantProvisioner _tenantProvisioner;
    private readonly IModuleInstaller _moduleInstaller;
    private readonly IInstallerLockService _installerLockService;

    public ProvisioningService(
        IDatabaseProvisioner databaseProvisioner,
        IMigrationRunner migrationRunner,
        ISeedRunner seedRunner,
        ITenantProvisioner tenantProvisioner,
        IModuleInstaller moduleInstaller,
        IInstallerLockService installerLockService)
    {
        _databaseProvisioner = databaseProvisioner;
        _migrationRunner = migrationRunner;
        _seedRunner = seedRunner;
        _tenantProvisioner = tenantProvisioner;
        _moduleInstaller = moduleInstaller;
        _installerLockService = installerLockService;
    }

    public async Task<ProvisioningResult> InstallAsync(InstallRequest request, CancellationToken cancellationToken = default)
    {
        var validationErrors = Validate(request);

        if (validationErrors.Count > 0)
        {
            return ProvisioningResult.Failure(validationErrors);
        }

        if (await _installerLockService.IsInstalledAsync(cancellationToken))
        {
            return ProvisioningResult.Failure([ProvisioningErrors.AlreadyInstalled]);
        }

        await _databaseProvisioner.TestConnectionAsync(request.Database, cancellationToken);
        await _databaseProvisioner.EnsureDatabaseCreatedAsync(request.Database, cancellationToken);
        await _migrationRunner.RunAsync(cancellationToken);
        await _seedRunner.RunAsync(cancellationToken);

        var tenantId = await _tenantProvisioner.CreateTenantAsync(request.Tenant, cancellationToken);
        await _tenantProvisioner.CreateBrandingAsync(tenantId, request.Branding, cancellationToken);
        var adminUserId = await _tenantProvisioner.CreateAdminUserAsync(tenantId, request.AdminUser, cancellationToken);

        var moduleSlugs = ResolveModuleSlugs(request.Modules);
        await _moduleInstaller.InstallBaseModulesAsync(tenantId, moduleSlugs, cancellationToken);

        await _installerLockService.LockAsync(InstallerDefaults.Version, InstallerDefaults.InstalledBy, cancellationToken);

        return ProvisioningResult.Success(tenantId, adminUserId, moduleSlugs);
    }

    private static IReadOnlyCollection<string> Validate(InstallRequest request)
    {
        var errors = new List<string>();

        AddIfEmpty(request.Database.Host, ProvisioningErrors.DatabaseHostRequired, errors);
        AddIfEmpty(request.Database.DatabaseName, ProvisioningErrors.DatabaseNameRequired, errors);
        AddIfEmpty(request.Database.Username, ProvisioningErrors.DatabaseUsernameRequired, errors);
        AddIfEmpty(request.Database.Password, ProvisioningErrors.DatabasePasswordRequired, errors);
        AddIfEmpty(request.Tenant.CompanyName, ProvisioningErrors.TenantCompanyNameRequired, errors);
        AddIfEmpty(request.Tenant.Slug, ProvisioningErrors.TenantSlugRequired, errors);
        AddIfEmpty(request.AdminUser.Name, ProvisioningErrors.AdminNameRequired, errors);
        AddIfEmpty(request.AdminUser.Email, ProvisioningErrors.AdminEmailRequired, errors);
        AddIfEmpty(request.AdminUser.Password, ProvisioningErrors.AdminPasswordRequired, errors);
        AddIfEmpty(request.Branding.SystemName, ProvisioningErrors.BrandingSystemNameRequired, errors);

        return errors;
    }

    private static void AddIfEmpty(string? value, string error, ICollection<string> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(error);
        }
    }

    private static IReadOnlyCollection<string> ResolveModuleSlugs(IReadOnlyCollection<string> requestedModules)
    {
        if (requestedModules.Count == 0)
        {
            return KnownModules.DefaultSlugs;
        }

        return requestedModules
            .Select(module => module.Trim().ToLowerInvariant())
            .Where(module => !string.IsNullOrWhiteSpace(module))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }
}
