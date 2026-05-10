using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Contacts;

public sealed class ContactRepository : IContactRepository
{
    private readonly AppDbContext _dbContext;

    public ContactRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Contact>> ListByCustomerAsync(
        Guid tenantId,
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Contacts
            .Where(contact => contact.TenantId == tenantId && contact.CustomerId == customerId)
            .OrderByDescending(contact => contact.IsPrimary)
            .ThenBy(contact => contact.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Contact?> GetByIdAsync(Guid tenantId, Guid contactId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Contacts
            .SingleOrDefaultAsync(contact => contact.TenantId == tenantId && contact.Id == contactId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Contact>> ListPrimaryByCustomerAsync(
        Guid tenantId,
        Guid customerId,
        Guid? exceptContactId = null,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Contacts
            .Where(contact =>
                contact.TenantId == tenantId &&
                contact.CustomerId == customerId &&
                contact.IsPrimary &&
                (exceptContactId == null || contact.Id != exceptContactId))
            .ToListAsync(cancellationToken);
    }

    public Task<bool> CustomerExistsAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Customers
            .AnyAsync(customer => customer.TenantId == tenantId && customer.Id == customerId, cancellationToken);
    }

    public Task<bool> EmailExistsAsync(
        Guid tenantId,
        Guid customerId,
        string email,
        Guid? exceptContactId = null,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Contacts.AnyAsync(contact =>
            contact.TenantId == tenantId &&
            contact.CustomerId == customerId &&
            contact.Email == email &&
            (exceptContactId == null || contact.Id != exceptContactId),
            cancellationToken);
    }

    public void Add(Contact contact)
    {
        _dbContext.Contacts.Add(contact);
    }

    public void Remove(Contact contact)
    {
        _dbContext.Contacts.Remove(contact);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
