using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Finance;

public sealed class FinancialTransactionRepository : IFinancialTransactionRepository
{
    private readonly AppDbContext _dbContext;

    public FinancialTransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<FinancialTransaction>> ListByAccountAsync(
        Guid tenantId,
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.FinancialTransactions
            .Where(transaction => transaction.TenantId == tenantId && transaction.AccountId == accountId)
            .OrderByDescending(transaction => transaction.OccurredOn)
            .ThenByDescending(transaction => transaction.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<FinancialTransaction?> GetByIdAsync(
        Guid tenantId,
        Guid transactionId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.FinancialTransactions
            .SingleOrDefaultAsync(transaction => transaction.TenantId == tenantId && transaction.Id == transactionId, cancellationToken);
    }

    public Task<FinancialAccount?> GetAccountByIdAsync(
        Guid tenantId,
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.FinancialAccounts
            .SingleOrDefaultAsync(account => account.TenantId == tenantId && account.Id == accountId, cancellationToken);
    }

    public void Add(FinancialTransaction transaction)
    {
        _dbContext.FinancialTransactions.Add(transaction);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
