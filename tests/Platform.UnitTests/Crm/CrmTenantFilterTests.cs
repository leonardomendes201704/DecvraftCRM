using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;
using Platform.Persistence;

namespace Platform.UnitTests.Crm;

public sealed class CrmTenantFilterTests
{
    [Fact]
    public async Task AppDbContext_filters_crm_entities_by_current_tenant()
    {
        var firstTenantId = Guid.NewGuid();
        var secondTenantId = Guid.NewGuid();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using (var seedContext = new AppDbContext(options))
        {
            seedContext.Customers.Add(Customer.Create(
                firstTenantId,
                "First tenant customer",
                "DOC-1",
                CustomerType.Company,
                DateTimeOffset.UtcNow));
            seedContext.Customers.Add(Customer.Create(
                secondTenantId,
                "Second tenant customer",
                "DOC-2",
                CustomerType.Company,
                DateTimeOffset.UtcNow));
            await seedContext.SaveChangesAsync();
        }

        await using var filteredContext = new AppDbContext(options, new FixedCurrentTenant(firstTenantId));

        var customers = await filteredContext.Customers.ToListAsync();

        var customer = Assert.Single(customers);
        Assert.Equal(firstTenantId, customer.TenantId);
    }

    private sealed class FixedCurrentTenant : ICurrentTenant
    {
        public FixedCurrentTenant(Guid tenantId)
        {
            TenantId = tenantId;
        }

        public Guid? TenantId { get; }
    }
}
