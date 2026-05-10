using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed record OpportunityActivityResponse(
    Guid Id,
    Guid TenantId,
    Guid OpportunityId,
    Guid? OwnerEmployeeId,
    OpportunityActivityType Type,
    string Title,
    string? Notes,
    DateTimeOffset? DueAt,
    OpportunityActivityStatus Status,
    DateTimeOffset? CompletedAt,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
