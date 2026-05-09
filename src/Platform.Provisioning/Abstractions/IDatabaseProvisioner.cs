using Platform.Provisioning.Models;

namespace Platform.Provisioning.Abstractions;

public interface IDatabaseProvisioner
{
    Task TestConnectionAsync(DatabaseSetupOptions options, CancellationToken cancellationToken = default);
    Task EnsureDatabaseCreatedAsync(DatabaseSetupOptions options, CancellationToken cancellationToken = default);
}
