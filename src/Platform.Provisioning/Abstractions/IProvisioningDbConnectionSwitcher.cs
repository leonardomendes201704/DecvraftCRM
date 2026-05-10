using Platform.Provisioning.Models;

namespace Platform.Provisioning.Abstractions;

public interface IProvisioningDbConnectionSwitcher
{
    Task UseTargetDatabaseAsync(DatabaseSetupOptions options, CancellationToken cancellationToken = default);
}
