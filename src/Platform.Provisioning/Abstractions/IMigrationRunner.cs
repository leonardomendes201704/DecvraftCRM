namespace Platform.Provisioning.Abstractions;

public interface IMigrationRunner
{
    Task RunAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetPendingMigrationsAsync(CancellationToken cancellationToken = default);
}
