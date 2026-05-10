using Platform.Domain.Common;
using Platform.Domain.Enums;

namespace Platform.Domain.Entities;

public sealed class FinancialTransaction : ITenantEntity
{
    private FinancialTransaction()
    {
    }

    private FinancialTransaction(
        Guid tenantId,
        Guid accountId,
        string description,
        decimal amount,
        FinancialTransactionType type,
        DateOnly occurredOn,
        DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        AccountId = accountId;
        Description = description;
        Amount = amount;
        Type = type;
        OccurredOn = occurredOn;
        Status = FinancialTransactionStatus.Posted;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid AccountId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public FinancialTransactionType Type { get; private set; }
    public DateOnly OccurredOn { get; private set; }
    public FinancialTransactionStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public static FinancialTransaction Create(
        Guid tenantId,
        Guid accountId,
        string description,
        decimal amount,
        FinancialTransactionType type,
        DateOnly occurredOn,
        DateTimeOffset createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ValidateAmount(amount);

        return new FinancialTransaction(tenantId, accountId, description.Trim(), amount, type, occurredOn, createdAt);
    }

    public void Update(
        string description,
        decimal amount,
        FinancialTransactionType type,
        DateOnly occurredOn,
        DateTimeOffset updatedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ValidateAmount(amount);

        Description = description.Trim();
        Amount = amount;
        Type = type;
        OccurredOn = occurredOn;
        UpdatedAt = updatedAt;
    }

    public void Void(DateTimeOffset updatedAt)
    {
        Status = FinancialTransactionStatus.Voided;
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
