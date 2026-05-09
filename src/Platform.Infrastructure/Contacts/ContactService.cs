using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Application.Contacts;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Contacts;

public sealed class ContactService : IContactService
{
    private readonly AppDbContext _dbContext;

    public ContactService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<ContactResponse>> ListByCustomerAsync(
        Guid tenantId,
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        var customerExists = await _dbContext.Customers
            .AnyAsync(customer => customer.TenantId == tenantId && customer.Id == customerId, cancellationToken);

        if (!customerExists)
        {
            return [];
        }

        return await _dbContext.Contacts
            .Where(contact => contact.TenantId == tenantId && contact.CustomerId == customerId)
            .OrderByDescending(contact => contact.IsPrimary)
            .ThenBy(contact => contact.Name)
            .Select(contact => new ContactResponse(
                contact.Id,
                contact.TenantId,
                contact.CustomerId,
                contact.Name,
                contact.Email,
                contact.Phone,
                contact.Role,
                contact.IsPrimary,
                contact.CreatedAt,
                contact.UpdatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<ContactResponse?> GetByIdAsync(
        Guid tenantId,
        Guid contactId,
        CancellationToken cancellationToken = default)
    {
        var contact = await _dbContext.Contacts
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == contactId, cancellationToken);

        return contact is null ? null : ToResponse(contact);
    }

    public async Task<ContactOperationResult> CreateAsync(
        Guid tenantId,
        Guid customerId,
        CreateContactRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
        {
            return ContactOperationResult.InvalidInput();
        }

        var customerExists = await _dbContext.Customers
            .AnyAsync(customer => customer.TenantId == tenantId && customer.Id == customerId, cancellationToken);

        if (!customerExists)
        {
            return ContactOperationResult.CustomerNotFound();
        }

        var email = request.Email.Trim().ToLowerInvariant();
        var duplicate = await _dbContext.Contacts
            .AnyAsync(contact =>
                contact.TenantId == tenantId &&
                contact.CustomerId == customerId &&
                contact.Email == email,
                cancellationToken);

        if (duplicate)
        {
            return ContactOperationResult.DuplicateEmail();
        }

        var contact = Contact.Create(
            tenantId,
            customerId,
            request.Name,
            email,
            request.Phone,
            request.Role,
            DateTimeOffset.UtcNow);

        if (request.IsPrimary)
        {
            contact.MarkAsPrimary(DateTimeOffset.UtcNow);
        }

        _dbContext.Contacts.Add(contact);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ContactOperationResult.Success(ToResponse(contact));
    }

    public async Task<ContactOperationResult> UpdateAsync(
        Guid tenantId,
        Guid contactId,
        UpdateContactRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
        {
            return ContactOperationResult.InvalidInput();
        }

        var contact = await _dbContext.Contacts
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == contactId, cancellationToken);

        if (contact is null)
        {
            return ContactOperationResult.NotFound();
        }

        var email = request.Email.Trim().ToLowerInvariant();
        var duplicate = await _dbContext.Contacts
            .AnyAsync(item =>
                item.TenantId == tenantId &&
                item.CustomerId == contact.CustomerId &&
                item.Id != contactId &&
                item.Email == email,
                cancellationToken);

        if (duplicate)
        {
            return ContactOperationResult.DuplicateEmail();
        }

        contact.Update(
            request.Name,
            email,
            request.Phone,
            request.Role,
            request.IsPrimary,
            DateTimeOffset.UtcNow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ContactOperationResult.Success(ToResponse(contact));
    }

    public async Task<ContactOperationResult> DeleteAsync(
        Guid tenantId,
        Guid contactId,
        CancellationToken cancellationToken = default)
    {
        var contact = await _dbContext.Contacts
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == contactId, cancellationToken);

        if (contact is null)
        {
            return ContactOperationResult.NotFound();
        }

        _dbContext.Contacts.Remove(contact);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ContactOperationResult.Success(ToResponse(contact));
    }

    private static ContactResponse ToResponse(Contact contact)
    {
        return new ContactResponse(
            contact.Id,
            contact.TenantId,
            contact.CustomerId,
            contact.Name,
            contact.Email,
            contact.Phone,
            contact.Role,
            contact.IsPrimary,
            contact.CreatedAt,
            contact.UpdatedAt);
    }
}
