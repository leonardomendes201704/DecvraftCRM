namespace Platform.Provisioning.Abstractions;

public interface IModuleInstaller
{
    Task InstallBaseModulesAsync(Guid tenantId, IReadOnlyCollection<string> moduleSlugs, CancellationToken cancellationToken = default);
}
