using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.UnitTests.Finance;

public sealed class FinanceDomainTests
{
    [Fact]
    public void FinancialAccount_apply_and_reverse_update_balance()
    {
        var account = FinancialAccount.Create(Guid.NewGuid(), "Caixa", 100, DateTimeOffset.UtcNow);

        account.Apply(FinancialTransactionType.Credit, 50, DateTimeOffset.UtcNow);
        account.Apply(FinancialTransactionType.Debit, 20, DateTimeOffset.UtcNow);
        account.Reverse(FinancialTransactionType.Credit, 10, DateTimeOffset.UtcNow);

        Assert.Equal(120, account.CurrentBalance);
    }

    [Fact]
    public void FinancialTransaction_Create_rejects_zero_amount()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            FinancialTransaction.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Invalid",
                0,
                FinancialTransactionType.Credit,
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Finance_enums_have_stable_values()
    {
        Assert.Equal(1, (int)FinancialAccountStatus.Active);
        Assert.Equal(2, (int)FinancialAccountStatus.Inactive);
        Assert.Equal(1, (int)FinancialTransactionType.Credit);
        Assert.Equal(2, (int)FinancialTransactionType.Debit);
        Assert.Equal(1, (int)FinancialTransactionStatus.Posted);
        Assert.Equal(2, (int)FinancialTransactionStatus.Voided);
    }
}
