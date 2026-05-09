using Microsoft.EntityFrameworkCore;
using Platform.Application.Customers;
using Platform.Domain.Entities;
using Platform.Domain.Enums;
using Platform.Infrastructure.Customers;
using Platform.Persistence;

namespace Platform.UnitTests.Customers;

public sealed class CustomerServiceTests
{
    [Fact]
    public async Task CreateAsync_creates_customer_for_tenant()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var service = new CustomerService(dbContext);

        var result = await service.CreateAsync(
            tenantId,
            new CreateCustomerRequest("Acme", "12345678000190", CustomerType.Company));

        Assert.Equal(CustomerOperationStatus.Success, result.Status);
        Assert.NotNull(result.Customer);
        Assert.Equal(tenantId, result.Customer.TenantId);
        Assert.Equal(CustomerStatus.Active, result.Customer.Status);
        Assert.Equal(1, await dbContext.Customers.CountAsync());
    }

    [Fact]
    public async Task CreateAsync_rejects_duplicate_document_inside_same_tenant()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var service = new CustomerService(dbContext);
        await service.CreateAsync(tenantId, new CreateCustomerRequest("Acme", "DOC-1", CustomerType.Company));

        var result = await service.CreateAsync(
            tenantId,
            new CreateCustomerRequest("Acme Duplicate", "DOC-1", CustomerType.Company));

        Assert.Equal(CustomerOperationStatus.DuplicateDocument, result.Status);
    }

    [Fact]
    public async Task ListAsync_returns_only_requested_tenant_customers()
    {
        await using var dbContext = CreateDbContext();
        var firstTenantId = Guid.NewGuid();
        var secondTenantId = Guid.NewGuid();
        dbContext.Customers.Add(Customer.Create(firstTenantId, "Acme", "DOC-1", CustomerType.Company, DateTimeOffset.UtcNow));
        dbContext.Customers.Add(Customer.Create(secondTenantId, "Beta", "DOC-2", CustomerType.Company, DateTimeOffset.UtcNow));
        await dbContext.SaveChangesAsync();
        var service = new CustomerService(dbContext);

        var customers = await service.ListAsync(firstTenantId);

        var customer = Assert.Single(customers);
        Assert.Equal("Acme", customer.Name);
    }

    [Fact]
    public async Task UpdateAsync_updates_existing_customer()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var customer = Customer.Create(tenantId, "Acme", "DOC-1", CustomerType.Company, DateTimeOffset.UtcNow);
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();
        var service = new CustomerService(dbContext);

        var result = await service.UpdateAsync(
            tenantId,
            customer.Id,
            new UpdateCustomerRequest("Acme Updated", "DOC-2", CustomerType.Individual));

        Assert.Equal(CustomerOperationStatus.Success, result.Status);
        Assert.Equal("Acme Updated", result.Customer!.Name);
        Assert.Equal("DOC-2", result.Customer.Document);
        Assert.Equal(CustomerType.Individual, result.Customer.Type);
    }

    [Fact]
    public async Task DeactivateAsync_marks_customer_as_inactive()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var customer = Customer.Create(tenantId, "Acme", "DOC-1", CustomerType.Company, DateTimeOffset.UtcNow);
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();
        var service = new CustomerService(dbContext);

        var result = await service.DeactivateAsync(tenantId, customer.Id);

        Assert.Equal(CustomerOperationStatus.Success, result.Status);
        Assert.Equal(CustomerStatus.Inactive, result.Customer!.Status);
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
