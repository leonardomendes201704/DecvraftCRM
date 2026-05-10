using Platform.Domain.Entities;

namespace Platform.Application.Abstractions;

public interface IFinancialTransactionRepository
{
    Task<IReadOnlyCollection<FinancialTransaction>> ListByAccountAsync(
        Guid tenantId,
        Guid accountId,
        CancellationToken cancellationToken = default);

    Task<FinancialTransaction?> GetByIdAsync(
        Guid tenantId,
        Guid transactionId,
        CancellationToken cancellationToken = default);

    Task<FinancialAccount?> GetAccountByIdAsync(
        Guid tenantId,
        Guid accountId,
        CancellationToken cancellationToken = default);

    void Add(FinancialTransaction transaction);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
