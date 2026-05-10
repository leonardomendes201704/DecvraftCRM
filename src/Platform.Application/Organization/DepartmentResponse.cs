namespace Platform.Application.Organization;

public sealed record DepartmentResponse(
    Guid Id,
    Guid TenantId,
    string Name,
    string Code,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
