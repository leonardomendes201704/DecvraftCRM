using Microsoft.Data.SqlClient;
using Platform.Provisioning.Abstractions;
using Platform.Provisioning.Database;
using Platform.Provisioning.Models;

namespace Platform.Provisioning.Services;

public sealed class DatabaseProvisioner : IDatabaseProvisioner
{
    private const string DatabaseExistsSql = "SELECT DB_ID(@databaseName)";
    private const string DatabaseNameParameter = "@databaseName";

    public async Task TestConnectionAsync(DatabaseSetupOptions options, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(SqlServerBootstrapConnectionFactory.CreateMasterConnectionString(options));
        await connection.OpenAsync(cancellationToken);
    }

    public async Task EnsureDatabaseCreatedAsync(DatabaseSetupOptions options, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(SqlServerBootstrapConnectionFactory.CreateMasterConnectionString(options));
        await connection.OpenAsync(cancellationToken);

        if (await DatabaseExistsAsync(connection, options.DatabaseName, cancellationToken))
        {
            return;
        }

        await using var command = connection.CreateCommand();
        command.CommandText = $"CREATE DATABASE {SqlServerIdentifier.Quote(options.DatabaseName)}";

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task<bool> DatabaseExistsAsync(SqlConnection connection, string databaseName, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = DatabaseExistsSql;
        command.Parameters.AddWithValue(DatabaseNameParameter, databaseName);

        var result = await command.ExecuteScalarAsync(cancellationToken);

        return result is not null && result is not DBNull;
    }
}
