using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Finance;

public sealed class FinancialAccountRepository : IFinancialAccountRepository
{
    private readonly AppDbContext _dbContext;

    public FinancialAccountRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<FinancialAccount>> ListAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.FinancialAccounts
            .Where(account => account.TenantId == tenantId)
            .OrderBy(account => account.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<FinancialAccount?> GetByIdAsync(Guid tenantId, Guid accountId, CancellationToken cancellationToken = default)
    {
        return _dbContext.FinancialAccounts
            .SingleOrDefaultAsync(account => account.TenantId == tenantId && account.Id == accountId, cancellationToken);
    }

    public Task<bool> NameExistsAsync(
        Guid tenantId,
        string name,
        Guid? exceptAccountId = null,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.FinancialAccounts.AnyAsync(account =>
            account.TenantId == tenantId &&
            account.Name == name &&
            (exceptAccountId == null || account.Id != exceptAccountId),
            cancellationToken);
    }

    public void Add(FinancialAccount account)
    {
        _dbContext.FinancialAccounts.Add(account);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
