using Platform.Provisioning.Models;

namespace Platform.Provisioning.Abstractions;

public interface IInstallerLockService
{
    Task<InstallStatusResult> GetStatusAsync(CancellationToken cancellationToken = default);
    Task<bool> IsInstalledAsync(CancellationToken cancellationToken = default);
    Task LockAsync(string installedVersion, string installedBy, CancellationToken cancellationToken = default);
}
