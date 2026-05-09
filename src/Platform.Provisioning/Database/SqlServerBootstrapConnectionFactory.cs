using Microsoft.Data.SqlClient;
using Platform.Provisioning.Models;

namespace Platform.Provisioning.Database;

public static class SqlServerBootstrapConnectionFactory
{
    private const string MasterDatabaseName = "master";

    public static string CreateMasterConnectionString(DatabaseSetupOptions options)
    {
        return CreateConnectionString(options, MasterDatabaseName);
    }

    public static string CreateTargetConnectionString(DatabaseSetupOptions options)
    {
        return CreateConnectionString(options, options.DatabaseName);
    }

    private static string CreateConnectionString(DatabaseSetupOptions options, string databaseName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Host);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Username);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Password);

        var builder = new SqlConnectionStringBuilder
        {
            DataSource = $"{options.Host},{options.Port}",
            InitialCatalog = databaseName,
            UserID = options.Username,
            Password = options.Password,
            TrustServerCertificate = options.TrustServerCertificate,
            Encrypt = true,
            ConnectTimeout = 15
        };

        return builder.ConnectionString;
    }
}
