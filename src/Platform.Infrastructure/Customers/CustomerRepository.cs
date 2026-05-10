using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Customers;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _dbContext;

    public CustomerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Customer>> ListAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .Where(customer => customer.TenantId == tenantId)
            .OrderBy(customer => customer.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Customer?> GetByIdAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Customers
            .SingleOrDefaultAsync(customer => customer.TenantId == tenantId && customer.Id == customerId, cancellationToken);
    }

    public Task<bool> DocumentExistsAsync(
        Guid tenantId,
        string document,
        Guid? exceptCustomerId = null,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Customers.AnyAsync(customer =>
            customer.TenantId == tenantId &&
            customer.Document == document &&
            (exceptCustomerId == null || customer.Id != exceptCustomerId),
            cancellationToken);
    }

    public void Add(Customer customer)
    {
        _dbContext.Customers.Add(customer);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
