using Platform.Domain.Entities;

namespace Platform.Application.Abstractions;

public interface ICustomerRepository
{
    Task<IReadOnlyCollection<Customer>> ListAsync(Guid tenantId, CancellationToken cancellationToken = default);

    Task<Customer?> GetByIdAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default);

    Task<bool> DocumentExistsAsync(
        Guid tenantId,
        string document,
        Guid? exceptCustomerId = null,
        CancellationToken cancellationToken = default);

    void Add(Customer customer);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
