using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Application.Finance;
using Platform.Domain.Entities;
using Platform.Domain.Enums;
using Platform.Persistence;

namespace Platform.Infrastructure.Finance;

public sealed class FinancialTransactionService : IFinancialTransactionService
{
    private readonly AppDbContext _dbContext;

    public FinancialTransactionService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<FinancialTransactionResponse>> ListByAccountAsync(
        Guid tenantId,
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var accountExists = await _dbContext.FinancialAccounts
            .AnyAsync(account => account.TenantId == tenantId && account.Id == accountId, cancellationToken);

        if (!accountExists)
        {
            return [];
        }

        return await _dbContext.FinancialTransactions
            .Where(transaction => transaction.TenantId == tenantId && transaction.AccountId == accountId)
            .OrderByDescending(transaction => transaction.OccurredOn)
            .ThenByDescending(transaction => transaction.CreatedAt)
            .Select(transaction => new FinancialTransactionResponse(
                transaction.Id,
                transaction.TenantId,
                transaction.AccountId,
                transaction.Description,
                transaction.Amount,
                transaction.Type,
                transaction.OccurredOn,
                transaction.Status,
                transaction.CreatedAt,
                transaction.UpdatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<FinancialTransactionResponse?> GetByIdAsync(Guid tenantId, Guid transactionId, CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.FinancialTransactions
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == transactionId, cancellationToken);

        return transaction is null ? null : ToResponse(transaction);
    }

    public async Task<FinancialTransactionOperationResult> CreateAsync(
        Guid tenantId,
        Guid accountId,
        CreateFinancialTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Description) || request.Amount <= decimal.Zero)
        {
            return FinancialTransactionOperationResult.InvalidInput();
        }

        var account = await _dbContext.FinancialAccounts
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == accountId, cancellationToken);

        if (account is null)
        {
            return FinancialTransactionOperationResult.AccountNotFound();
        }

        var transaction = FinancialTransaction.Create(
            tenantId,
            accountId,
            request.Description,
            request.Amount,
            request.Type,
            request.OccurredOn,
            DateTimeOffset.UtcNow);

        account.Apply(transaction.Type, transaction.Amount, DateTimeOffset.UtcNow);
        _dbContext.FinancialTransactions.Add(transaction);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return FinancialTransactionOperationResult.Success(ToResponse(transaction));
    }

    public async Task<FinancialTransactionOperationResult> UpdateAsync(
        Guid tenantId,
        Guid transactionId,
        UpdateFinancialTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Description) || request.Amount <= decimal.Zero)
        {
            return FinancialTransactionOperationResult.InvalidInput();
        }

        var transaction = await _dbContext.FinancialTransactions
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == transactionId, cancellationToken);

        if (transaction is null)
        {
            return FinancialTransactionOperationResult.NotFound();
        }

        var account = await _dbContext.FinancialAccounts
            .SingleAsync(item => item.TenantId == tenantId && item.Id == transaction.AccountId, cancellationToken);

        if (transaction.Status == FinancialTransactionStatus.Posted)
        {
            account.Reverse(transaction.Type, transaction.Amount, DateTimeOffset.UtcNow);
            account.Apply(request.Type, request.Amount, DateTimeOffset.UtcNow);
        }

        transaction.Update(
            request.Description,
            request.Amount,
            request.Type,
            request.OccurredOn,
            DateTimeOffset.UtcNow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return FinancialTransactionOperationResult.Success(ToResponse(transaction));
    }

    public async Task<FinancialTransactionOperationResult> VoidAsync(
        Guid tenantId,
        Guid transactionId,
        CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.FinancialTransactions
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == transactionId, cancellationToken);

        if (transaction is null)
        {
            return FinancialTransactionOperationResult.NotFound();
        }

        if (transaction.Status == FinancialTransactionStatus.Posted)
        {
            var account = await _dbContext.FinancialAccounts
                .SingleAsync(item => item.TenantId == tenantId && item.Id == transaction.AccountId, cancellationToken);
            account.Reverse(transaction.Type, transaction.Amount, DateTimeOffset.UtcNow);
            transaction.Void(DateTimeOffset.UtcNow);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return FinancialTransactionOperationResult.Success(ToResponse(transaction));
    }

    private static FinancialTransactionResponse ToResponse(FinancialTransaction transaction)
    {
        return new FinancialTransactionResponse(
            transaction.Id,
            transaction.TenantId,
            transaction.AccountId,
            transaction.Description,
            transaction.Amount,
            transaction.Type,
            transaction.OccurredOn,
            transaction.Status,
            transaction.CreatedAt,
            transaction.UpdatedAt);
    }
}
