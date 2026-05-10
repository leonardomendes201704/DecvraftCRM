namespace Platform.Application.Organization;

public sealed record JobTitleResponse(
    Guid Id,
    Guid TenantId,
    string Name,
    string Code,
    int Level,
    bool IsLeadership,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
