using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed record OpportunityResponse(
    Guid Id,
    Guid TenantId,
    Guid CustomerId,
    Guid? StageId,
    string Title,
    decimal EstimatedValue,
    DateOnly? ExpectedCloseDate,
    OpportunityStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
