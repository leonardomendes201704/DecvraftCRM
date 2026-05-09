using Microsoft.Data.SqlClient;
using Platform.Provisioning.Database;
using Platform.Provisioning.Models;

namespace Platform.ProvisioningTests;

public sealed class DatabaseProvisionerTests
{
    private const string Host = "localhost";
    private const int Port = 1433;
    private const string DatabaseName = "WhiteLabelErp";
    private const string Username = "sa";
    private const string Password = "bootstrap-only";
    private const string MasterDatabaseName = "master";
    private const string UnsafeIdentifier = "Tenant]Db";
    private const string QuotedUnsafeIdentifier = "[Tenant]]Db]";

    [Fact]
    public void CreateMasterConnectionString_ShouldTargetMasterDatabase()
    {
        var connectionString = SqlServerBootstrapConnectionFactory.CreateMasterConnectionString(CreateOptions());
        var builder = new SqlConnectionStringBuilder(connectionString);

        Assert.Equal($"{Host},{Port}", builder.DataSource);
        Assert.Equal(MasterDatabaseName, builder.InitialCatalog);
        Assert.Equal(Username, builder.UserID);
        Assert.Equal(Password, builder.Password);
        Assert.True(builder.TrustServerCertificate);
        Assert.True(builder.Encrypt);
    }

    [Fact]
    public void CreateTargetConnectionString_ShouldTargetRequestedDatabase()
    {
        var connectionString = SqlServerBootstrapConnectionFactory.CreateTargetConnectionString(CreateOptions());
        var builder = new SqlConnectionStringBuilder(connectionString);

        Assert.Equal(DatabaseName, builder.InitialCatalog);
    }

    [Fact]
    public void Quote_ShouldEscapeClosingBracket()
    {
        var quotedIdentifier = SqlServerIdentifier.Quote(UnsafeIdentifier);

        Assert.Equal(QuotedUnsafeIdentifier, quotedIdentifier);
    }

    private static DatabaseSetupOptions CreateOptions()
    {
        return new DatabaseSetupOptions
        {
            Host = Host,
            Port = Port,
            DatabaseName = DatabaseName,
            Username = Username,
            Password = Password,
            TrustServerCertificate = true
        };
    }
}
