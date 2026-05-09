using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Application.Customers;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Customers;

public sealed class CustomerService : ICustomerService
{
    private readonly AppDbContext _dbContext;

    public CustomerService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<CustomerResponse>> ListAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .Where(customer => customer.TenantId == tenantId)
            .OrderBy(customer => customer.Name)
            .Select(customer => new CustomerResponse(
                customer.Id,
                customer.TenantId,
                customer.Name,
                customer.Document,
                customer.Type,
                customer.Status,
                customer.CreatedAt,
                customer.UpdatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<CustomerResponse?> GetByIdAsync(
        Guid tenantId,
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        var customer = await _dbContext.Customers
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == customerId, cancellationToken);

        return customer is null ? null : ToResponse(customer);
    }

    public async Task<CustomerOperationResult> CreateAsync(
        Guid tenantId,
        CreateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Document))
        {
            return CustomerOperationResult.InvalidInput();
        }

        var document = request.Document.Trim();
        var exists = await _dbContext.Customers
            .AnyAsync(customer => customer.TenantId == tenantId && customer.Document == document, cancellationToken);

        if (exists)
        {
            return CustomerOperationResult.DuplicateDocument();
        }

        var customer = Customer.Create(
            tenantId,
            request.Name,
            document,
            request.Type,
            DateTimeOffset.UtcNow);

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return CustomerOperationResult.Success(ToResponse(customer));
    }

    public async Task<CustomerOperationResult> UpdateAsync(
        Guid tenantId,
        Guid customerId,
        UpdateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Document))
        {
            return CustomerOperationResult.InvalidInput();
        }

        var customer = await _dbContext.Customers
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == customerId, cancellationToken);

        if (customer is null)
        {
            return CustomerOperationResult.NotFound();
        }

        var document = request.Document.Trim();
        var documentInUse = await _dbContext.Customers
            .AnyAsync(item =>
                item.TenantId == tenantId &&
                item.Id != customerId &&
                item.Document == document,
                cancellationToken);

        if (documentInUse)
        {
            return CustomerOperationResult.DuplicateDocument();
        }

        customer.Update(request.Name, document, request.Type, DateTimeOffset.UtcNow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return CustomerOperationResult.Success(ToResponse(customer));
    }

    public async Task<CustomerOperationResult> DeactivateAsync(
        Guid tenantId,
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        var customer = await _dbContext.Customers
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == customerId, cancellationToken);

        if (customer is null)
        {
            return CustomerOperationResult.NotFound();
        }

        customer.Deactivate(DateTimeOffset.UtcNow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return CustomerOperationResult.Success(ToResponse(customer));
    }

    private static CustomerResponse ToResponse(Customer customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.TenantId,
            customer.Name,
            customer.Document,
            customer.Type,
            customer.Status,
            customer.CreatedAt,
            customer.UpdatedAt);
    }
}
