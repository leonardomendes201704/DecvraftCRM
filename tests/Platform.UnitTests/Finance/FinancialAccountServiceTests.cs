using Microsoft.EntityFrameworkCore;
using Platform.Application.Finance;
using Platform.Domain.Entities;
using Platform.Domain.Enums;
using Platform.Infrastructure.Finance;
using Platform.Persistence;

namespace Platform.UnitTests.Finance;

public sealed class FinancialAccountServiceTests
{
    [Fact]
    public async Task CreateAsync_creates_account_for_tenant()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var service = new FinancialAccountService(dbContext);

        var result = await service.CreateAsync(tenantId, new CreateFinancialAccountRequest("Caixa", 100));

        Assert.Equal(FinancialAccountOperationStatus.Success, result.Status);
        Assert.Equal(100, result.Account!.CurrentBalance);
    }

    [Fact]
    public async Task CreateAsync_rejects_duplicate_name_inside_same_tenant()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var service = new FinancialAccountService(dbContext);
        await service.CreateAsync(tenantId, new CreateFinancialAccountRequest("Caixa", 0));

        var result = await service.CreateAsync(tenantId, new CreateFinancialAccountRequest("Caixa", 0));

        Assert.Equal(FinancialAccountOperationStatus.DuplicateName, result.Status);
    }

    [Fact]
    public async Task ListAsync_returns_only_requested_tenant_accounts()
    {
        await using var dbContext = CreateDbContext();
        var firstTenantId = Guid.NewGuid();
        var secondTenantId = Guid.NewGuid();
        dbContext.FinancialAccounts.Add(FinancialAccount.Create(firstTenantId, "Caixa", 0, DateTimeOffset.UtcNow));
        dbContext.FinancialAccounts.Add(FinancialAccount.Create(secondTenantId, "Banco", 0, DateTimeOffset.UtcNow));
        await dbContext.SaveChangesAsync();
        var service = new FinancialAccountService(dbContext);

        var accounts = await service.ListAsync(firstTenantId);

        var account = Assert.Single(accounts);
        Assert.Equal("Caixa", account.Name);
    }

    [Fact]
    public async Task DeactivateAsync_marks_account_as_inactive()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var account = FinancialAccount.Create(tenantId, "Caixa", 0, DateTimeOffset.UtcNow);
        dbContext.FinancialAccounts.Add(account);
        await dbContext.SaveChangesAsync();
        var service = new FinancialAccountService(dbContext);

        var result = await service.DeactivateAsync(tenantId, account.Id);

        Assert.Equal(FinancialAccountOperationStatus.Success, result.Status);
        Assert.Equal(FinancialAccountStatus.Inactive, result.Account!.Status);
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
