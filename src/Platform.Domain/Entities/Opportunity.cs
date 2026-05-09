using Platform.Domain.Common;
using Platform.Domain.Enums;

namespace Platform.Domain.Entities;

public sealed class Opportunity : ITenantEntity
{
    private Opportunity()
    {
    }

    private Opportunity(
        Guid tenantId,
        Guid customerId,
        string title,
        decimal estimatedValue,
        DateOnly? expectedCloseDate,
        DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        CustomerId = customerId;
        Title = title;
        EstimatedValue = estimatedValue;
        ExpectedCloseDate = expectedCloseDate;
        Status = OpportunityStatus.Open;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid CustomerId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public decimal EstimatedValue { get; private set; }
    public DateOnly? ExpectedCloseDate { get; private set; }
    public OpportunityStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public static Opportunity Create(
        Guid tenantId,
        Guid customerId,
        string title,
        decimal estimatedValue,
        DateOnly? expectedCloseDate,
        DateTimeOffset createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        if (estimatedValue < decimal.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(estimatedValue));
        }

        return new Opportunity(
            tenantId,
            customerId,
            title.Trim(),
            estimatedValue,
            expectedCloseDate,
            createdAt);
    }

    public void MarkAsWon(DateTimeOffset updatedAt)
    {
        Status = OpportunityStatus.Won;
        UpdatedAt = updatedAt;
    }

    public void MarkAsLost(DateTimeOffset updatedAt)
    {
        Status = OpportunityStatus.Lost;
        UpdatedAt = updatedAt;
    }
}
