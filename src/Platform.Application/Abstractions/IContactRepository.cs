using Platform.Domain.Entities;

namespace Platform.Application.Abstractions;

public interface IContactRepository
{
    Task<IReadOnlyCollection<Contact>> ListByCustomerAsync(
        Guid tenantId,
        Guid customerId,
        CancellationToken cancellationToken = default);

    Task<Contact?> GetByIdAsync(Guid tenantId, Guid contactId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Contact>> ListPrimaryByCustomerAsync(
        Guid tenantId,
        Guid customerId,
        Guid? exceptContactId = null,
        CancellationToken cancellationToken = default);

    Task<bool> CustomerExistsAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default);

    Task<bool> EmailExistsAsync(
        Guid tenantId,
        Guid customerId,
        string email,
        Guid? exceptContactId = null,
        CancellationToken cancellationToken = default);

    void Add(Contact contact);

    void Remove(Contact contact);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
