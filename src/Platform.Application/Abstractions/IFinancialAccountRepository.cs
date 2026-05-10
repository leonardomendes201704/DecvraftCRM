using Platform.Domain.Entities;

namespace Platform.Application.Abstractions;

public interface IFinancialAccountRepository
{
    Task<IReadOnlyCollection<FinancialAccount>> ListAsync(Guid tenantId, CancellationToken cancellationToken = default);

    Task<FinancialAccount?> GetByIdAsync(Guid tenantId, Guid accountId, CancellationToken cancellationToken = default);

    Task<bool> NameExistsAsync(
        Guid tenantId,
        string name,
        Guid? exceptAccountId = null,
        CancellationToken cancellationToken = default);

    void Add(FinancialAccount account);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
