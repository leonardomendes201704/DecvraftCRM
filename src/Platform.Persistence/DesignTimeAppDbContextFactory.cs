using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Platform.Persistence;

public sealed class DesignTimeAppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WhiteLabelErpCrmDesignTime;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        return new AppDbContext(options);
    }
}
