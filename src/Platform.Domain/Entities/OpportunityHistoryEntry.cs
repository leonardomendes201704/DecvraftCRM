using Platform.Domain.Common;
using Platform.Domain.Enums;

namespace Platform.Domain.Entities;

public sealed class OpportunityHistoryEntry : ITenantEntity
{
    private OpportunityHistoryEntry()
    {
    }

    private OpportunityHistoryEntry(
        Guid tenantId,
        Guid opportunityId,
        OpportunityHistoryEventType type,
        string description,
        DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        OpportunityId = opportunityId;
        Type = type;
        Description = description;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OpportunityId { get; private set; }
    public OpportunityHistoryEventType Type { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }

    public static OpportunityHistoryEntry Create(
        Guid tenantId,
        Guid opportunityId,
        OpportunityHistoryEventType type,
        string description,
        DateTimeOffset createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        return new OpportunityHistoryEntry(
            tenantId,
            opportunityId,
            type,
            description.Trim(),
            createdAt);
    }
}
