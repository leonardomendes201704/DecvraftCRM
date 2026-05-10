using Platform.Domain.Common;
using Platform.Domain.Enums;

namespace Platform.Domain.Entities;

public sealed class OpportunityActivity : ITenantEntity
{
    private OpportunityActivity()
    {
    }

    private OpportunityActivity(
        Guid tenantId,
        Guid opportunityId,
        OpportunityActivityType type,
        string title,
        string? notes,
        DateTimeOffset? dueAt,
        DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        OpportunityId = opportunityId;
        Type = type;
        Title = title;
        Notes = notes;
        DueAt = dueAt;
        Status = OpportunityActivityStatus.Scheduled;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OpportunityId { get; private set; }
    public OpportunityActivityType Type { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Notes { get; private set; }
    public DateTimeOffset? DueAt { get; private set; }
    public OpportunityActivityStatus Status { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public static OpportunityActivity Create(
        Guid tenantId,
        Guid opportunityId,
        OpportunityActivityType type,
        string title,
        string? notes,
        DateTimeOffset? dueAt,
        DateTimeOffset createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        return new OpportunityActivity(
            tenantId,
            opportunityId,
            type,
            title.Trim(),
            Normalize(notes),
            dueAt,
            createdAt);
    }

    public void Update(
        OpportunityActivityType type,
        string title,
        string? notes,
        DateTimeOffset? dueAt,
        DateTimeOffset updatedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        Type = type;
        Title = title.Trim();
        Notes = Normalize(notes);
        DueAt = dueAt;
        UpdatedAt = updatedAt;
    }

    public void Complete(DateTimeOffset completedAt)
    {
        Status = OpportunityActivityStatus.Completed;
        CompletedAt = completedAt;
        UpdatedAt = completedAt;
    }

    public void Cancel(DateTimeOffset updatedAt)
    {
        Status = OpportunityActivityStatus.Canceled;
        UpdatedAt = updatedAt;
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
