using Microsoft.EntityFrameworkCore;
using Platform.Persistence;

namespace Platform.MigrationTests;

public sealed class PersistenceModelTests
{
    [Fact]
    public void AppDbContext_ShouldGenerateCreateScriptForCoreTables()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=localhost;Database=ModelValidation;User Id=sa;Password=placeholder;TrustServerCertificate=True;")
            .Options;

        using var context = new AppDbContext(options);

        var script = context.Database.GenerateCreateScript();

        Assert.Contains("Tenants", script);
        Assert.Contains("SystemInstallations", script);
        Assert.Contains("ApplicationUsers", script);
        Assert.Contains("TenantConfigurations", script);
    }
}
