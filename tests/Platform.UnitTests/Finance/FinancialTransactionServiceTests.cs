using Microsoft.EntityFrameworkCore;
using Platform.Application.Finance;
using Platform.Domain.Entities;
using Platform.Domain.Enums;
using Platform.Infrastructure.Finance;
using Platform.Persistence;

namespace Platform.UnitTests.Finance;

public sealed class FinancialTransactionServiceTests
{
    [Fact]
    public async Task CreateAsync_posts_transaction_and_updates_account_balance()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var account = SeedAccount(dbContext, tenantId, openingBalance: 100);
        await dbContext.SaveChangesAsync();
        var service = new FinancialTransactionService(dbContext);

        var result = await service.CreateAsync(
            tenantId,
            account.Id,
            new CreateFinancialTransactionRequest("Receita", 50, FinancialTransactionType.Credit, new DateOnly(2026, 5, 10)));

        Assert.Equal(FinancialTransactionOperationStatus.Success, result.Status);
        Assert.Equal(FinancialTransactionStatus.Posted, result.Transaction!.Status);
        Assert.Equal(150, (await dbContext.FinancialAccounts.SingleAsync()).CurrentBalance);
    }

    [Fact]
    public async Task CreateAsync_returns_account_not_found_for_other_tenant_account()
    {
        await using var dbContext = CreateDbContext();
        var firstTenantId = Guid.NewGuid();
        var secondTenantId = Guid.NewGuid();
        var account = SeedAccount(dbContext, firstTenantId);
        await dbContext.SaveChangesAsync();
        var service = new FinancialTransactionService(dbContext);

        var result = await service.CreateAsync(
            secondTenantId,
            account.Id,
            new CreateFinancialTransactionRequest("Receita", 50, FinancialTransactionType.Credit, new DateOnly(2026, 5, 10)));

        Assert.Equal(FinancialTransactionOperationStatus.AccountNotFound, result.Status);
    }

    [Fact]
    public async Task UpdateAsync_recalculates_account_balance_for_posted_transaction()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var account = SeedAccount(dbContext, tenantId, openingBalance: 100);
        var transaction = FinancialTransaction.Create(
            tenantId,
            account.Id,
            "Receita",
            50,
            FinancialTransactionType.Credit,
            new DateOnly(2026, 5, 10),
            DateTimeOffset.UtcNow);
        account.Apply(transaction.Type, transaction.Amount, DateTimeOffset.UtcNow);
        dbContext.FinancialTransactions.Add(transaction);
        await dbContext.SaveChangesAsync();
        var service = new FinancialTransactionService(dbContext);

        var result = await service.UpdateAsync(
            tenantId,
            transaction.Id,
            new UpdateFinancialTransactionRequest("Despesa", 30, FinancialTransactionType.Debit, new DateOnly(2026, 5, 11)));

        Assert.Equal(FinancialTransactionOperationStatus.Success, result.Status);
        Assert.Equal(70, account.CurrentBalance);
    }

    [Fact]
    public async Task VoidAsync_reverses_posted_transaction_once()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var account = SeedAccount(dbContext, tenantId, openingBalance: 100);
        var transaction = FinancialTransaction.Create(
            tenantId,
            account.Id,
            "Receita",
            50,
            FinancialTransactionType.Credit,
            new DateOnly(2026, 5, 10),
            DateTimeOffset.UtcNow);
        account.Apply(transaction.Type, transaction.Amount, DateTimeOffset.UtcNow);
        dbContext.FinancialTransactions.Add(transaction);
        await dbContext.SaveChangesAsync();
        var service = new FinancialTransactionService(dbContext);

        var result = await service.VoidAsync(tenantId, transaction.Id);
        await service.VoidAsync(tenantId, transaction.Id);

        Assert.Equal(FinancialTransactionStatus.Voided, result.Transaction!.Status);
        Assert.Equal(100, account.CurrentBalance);
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static FinancialAccount SeedAccount(AppDbContext dbContext, Guid tenantId, decimal openingBalance = 0)
    {
        var account = FinancialAccount.Create(tenantId, "Caixa", openingBalance, DateTimeOffset.UtcNow);
        dbContext.FinancialAccounts.Add(account);

        return account;
    }
}
