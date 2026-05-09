using Microsoft.EntityFrameworkCore;
using Platform.Persistence;

namespace Platform.ProvisioningTests.Support;

internal static class ProvisioningTestDbContextFactory
{
    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
