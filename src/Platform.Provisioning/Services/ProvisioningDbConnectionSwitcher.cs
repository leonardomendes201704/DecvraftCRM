using Microsoft.EntityFrameworkCore;
using Platform.Persistence;
using Platform.Provisioning.Abstractions;
using Platform.Provisioning.Database;
using Platform.Provisioning.Models;

namespace Platform.Provisioning.Services;

public sealed class ProvisioningDbConnectionSwitcher : IProvisioningDbConnectionSwitcher
{
    private readonly AppDbContext _dbContext;

    public ProvisioningDbConnectionSwitcher(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task UseTargetDatabaseAsync(DatabaseSetupOptions options, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Database.GetDbConnection();

        if (connection.State != System.Data.ConnectionState.Closed)
        {
            await connection.CloseAsync();
        }

        connection.ConnectionString = SqlServerBootstrapConnectionFactory.CreateTargetConnectionString(options);
    }
}
