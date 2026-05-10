using Platform.Application.Finance;

namespace Platform.Application.Abstractions;

public interface IFinancialAccountService
{
    Task<IReadOnlyCollection<FinancialAccountResponse>> ListAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<FinancialAccountResponse?> GetByIdAsync(Guid tenantId, Guid accountId, CancellationToken cancellationToken = default);
    Task<FinancialAccountOperationResult> CreateAsync(Guid tenantId, CreateFinancialAccountRequest request, CancellationToken cancellationToken = default);
    Task<FinancialAccountOperationResult> UpdateAsync(Guid tenantId, Guid accountId, UpdateFinancialAccountRequest request, CancellationToken cancellationToken = default);
    Task<FinancialAccountOperationResult> DeactivateAsync(Guid tenantId, Guid accountId, CancellationToken cancellationToken = default);
}
