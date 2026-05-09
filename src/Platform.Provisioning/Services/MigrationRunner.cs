using Microsoft.EntityFrameworkCore;
using Platform.Persistence;
using Platform.Provisioning.Abstractions;

namespace Platform.Provisioning.Services;

public sealed class MigrationRunner : IMigrationRunner
{
    private readonly AppDbContext _dbContext;

    public MigrationRunner(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.MigrateAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<string>> GetPendingMigrationsAsync(CancellationToken cancellationToken = default)
    {
        var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken);

        return pendingMigrations.ToList();
    }
}
