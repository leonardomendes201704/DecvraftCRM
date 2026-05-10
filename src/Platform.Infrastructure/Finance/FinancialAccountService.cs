using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Application.Finance;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Finance;

public sealed class FinancialAccountService : IFinancialAccountService
{
    private readonly AppDbContext _dbContext;

    public FinancialAccountService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<FinancialAccountResponse>> ListAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FinancialAccounts
            .Where(account => account.TenantId == tenantId)
            .OrderBy(account => account.Name)
            .Select(account => new FinancialAccountResponse(
                account.Id,
                account.TenantId,
                account.Name,
                account.OpeningBalance,
                account.CurrentBalance,
                account.Status,
                account.CreatedAt,
                account.UpdatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<FinancialAccountResponse?> GetByIdAsync(Guid tenantId, Guid accountId, CancellationToken cancellationToken = default)
    {
        var account = await _dbContext.FinancialAccounts
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == accountId, cancellationToken);

        return account is null ? null : ToResponse(account);
    }

    public async Task<FinancialAccountOperationResult> CreateAsync(
        Guid tenantId,
        CreateFinancialAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return FinancialAccountOperationResult.InvalidInput();
        }

        var name = request.Name.Trim();
        var duplicate = await _dbContext.FinancialAccounts
            .AnyAsync(account => account.TenantId == tenantId && account.Name == name, cancellationToken);

        if (duplicate)
        {
            return FinancialAccountOperationResult.DuplicateName();
        }

        var account = FinancialAccount.Create(tenantId, name, request.OpeningBalance, DateTimeOffset.UtcNow);
        _dbContext.FinancialAccounts.Add(account);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return FinancialAccountOperationResult.Success(ToResponse(account));
    }

    public async Task<FinancialAccountOperationResult> UpdateAsync(
        Guid tenantId,
        Guid accountId,
        UpdateFinancialAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return FinancialAccountOperationResult.InvalidInput();
        }

        var account = await _dbContext.FinancialAccounts
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == accountId, cancellationToken);

        if (account is null)
        {
            return FinancialAccountOperationResult.NotFound();
        }

        var name = request.Name.Trim();
        var duplicate = await _dbContext.FinancialAccounts
            .AnyAsync(item => item.TenantId == tenantId && item.Id != accountId && item.Name == name, cancellationToken);

        if (duplicate)
        {
            return FinancialAccountOperationResult.DuplicateName();
        }

        account.Update(name, DateTimeOffset.UtcNow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return FinancialAccountOperationResult.Success(ToResponse(account));
    }

    public async Task<FinancialAccountOperationResult> DeactivateAsync(
        Guid tenantId,
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var account = await _dbContext.FinancialAccounts
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == accountId, cancellationToken);

        if (account is null)
        {
            return FinancialAccountOperationResult.NotFound();
        }

        account.Deactivate(DateTimeOffset.UtcNow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return FinancialAccountOperationResult.Success(ToResponse(account));
    }

    private static FinancialAccountResponse ToResponse(FinancialAccount account)
    {
        return new FinancialAccountResponse(
            account.Id,
            account.TenantId,
            account.Name,
            account.OpeningBalance,
            account.CurrentBalance,
            account.Status,
            account.CreatedAt,
            account.UpdatedAt);
    }
}
