using Platform.Domain.Enums;

namespace Platform.Application.Customers;

public sealed record CustomerResponse(
    Guid Id,
    Guid TenantId,
    string Name,
    string Document,
    CustomerType Type,
    CustomerStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
