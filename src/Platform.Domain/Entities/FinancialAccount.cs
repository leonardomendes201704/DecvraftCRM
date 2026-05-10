using Platform.Domain.Common;
using Platform.Domain.Enums;

namespace Platform.Domain.Entities;

public sealed class FinancialAccount : ITenantEntity
{
    private FinancialAccount()
    {
    }

    private FinancialAccount(Guid tenantId, string name, decimal openingBalance, DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Name = name;
        OpeningBalance = openingBalance;
        CurrentBalance = openingBalance;
        Status = FinancialAccountStatus.Active;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal OpeningBalance { get; private set; }
    public decimal CurrentBalance { get; private set; }
    public FinancialAccountStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public static FinancialAccount Create(Guid tenantId, string name, decimal openingBalance, DateTimeOffset createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new FinancialAccount(tenantId, name.Trim(), openingBalance, createdAt);
    }

    public void Update(string name, DateTimeOffset updatedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name.Trim();
        UpdatedAt = updatedAt;
    }

    public void Deactivate(DateTimeOffset updatedAt)
    {
        Status = FinancialAccountStatus.Inactive;
        UpdatedAt = updatedAt;
    }

    public void Apply(FinancialTransactionType type, decimal amount, DateTimeOffset updatedAt)
    {
        ValidateAmount(amount);

        CurrentBalance += type == FinancialTransactionType.Credit ? amount : -amount;
        UpdatedAt = updatedAt;
    }

    public void Reverse(FinancialTransactionType type, decimal amount, DateTimeOffset updatedAt)
    {
        ValidateAmount(amount);

        CurrentBalance += type == FinancialTransactionType.Credit ? -amount : amount;
        UpdatedAt = updatedAt;
    }

    private static void ValidateAmount(decimal amount)
    {
        if (amount <= decimal.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }
    }
}
