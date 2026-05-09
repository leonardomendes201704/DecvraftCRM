namespace Platform.Application.Contacts;

public sealed record ContactResponse(
    Guid Id,
    Guid TenantId,
    Guid CustomerId,
    string Name,
    string Email,
    string? Phone,
    string? Role,
    bool IsPrimary,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
