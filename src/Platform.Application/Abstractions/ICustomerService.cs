using Platform.Application.Customers;

namespace Platform.Application.Abstractions;

public interface ICustomerService
{
    Task<IReadOnlyCollection<CustomerResponse>> ListAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default);

    Task<CustomerResponse?> GetByIdAsync(
        Guid tenantId,
        Guid customerId,
        CancellationToken cancellationToken = default);

    Task<CustomerOperationResult> CreateAsync(
        Guid tenantId,
        CreateCustomerRequest request,
        CancellationToken cancellationToken = default);

    Task<CustomerOperationResult> UpdateAsync(
        Guid tenantId,
        Guid customerId,
        UpdateCustomerRequest request,
        CancellationToken cancellationToken = default);

    Task<CustomerOperationResult> DeactivateAsync(
        Guid tenantId,
        Guid customerId,
        CancellationToken cancellationToken = default);
}
