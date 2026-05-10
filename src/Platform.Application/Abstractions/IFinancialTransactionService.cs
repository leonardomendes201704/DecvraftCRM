using Platform.Application.Finance;

namespace Platform.Application.Abstractions;

public interface IFinancialTransactionService
{
    Task<IReadOnlyCollection<FinancialTransactionResponse>> ListByAccountAsync(Guid tenantId, Guid accountId, CancellationToken cancellationToken = default);
    Task<FinancialTransactionResponse?> GetByIdAsync(Guid tenantId, Guid transactionId, CancellationToken cancellationToken = default);
    Task<FinancialTransactionOperationResult> CreateAsync(Guid tenantId, Guid accountId, CreateFinancialTransactionRequest request, CancellationToken cancellationToken = default);
    Task<FinancialTransactionOperationResult> UpdateAsync(Guid tenantId, Guid transactionId, UpdateFinancialTransactionRequest request, CancellationToken cancellationToken = default);
    Task<FinancialTransactionOperationResult> VoidAsync(Guid tenantId, Guid transactionId, CancellationToken cancellationToken = default);
}
