using Platform.Application.Contacts;

namespace Platform.Application.Abstractions;

public interface IContactService
{
    Task<IReadOnlyCollection<ContactResponse>> ListByCustomerAsync(
        Guid tenantId,
        Guid customerId,
        CancellationToken cancellationToken = default);

    Task<ContactResponse?> GetByIdAsync(
        Guid tenantId,
        Guid contactId,
        CancellationToken cancellationToken = default);

    Task<ContactOperationResult> CreateAsync(
        Guid tenantId,
        Guid customerId,
        CreateContactRequest request,
        CancellationToken cancellationToken = default);

    Task<ContactOperationResult> UpdateAsync(
        Guid tenantId,
        Guid contactId,
        UpdateContactRequest request,
        CancellationToken cancellationToken = default);

    Task<ContactOperationResult> DeleteAsync(
        Guid tenantId,
        Guid contactId,
        CancellationToken cancellationToken = default);
}
