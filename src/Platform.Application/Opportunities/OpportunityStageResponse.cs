namespace Platform.Application.Opportunities;

public sealed record OpportunityStageResponse(
    Guid Id,
    Guid TenantId,
    string Name,
    int Position,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
