using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed record CreateOpportunityActivityRequest(
    OpportunityActivityType Type,
    string Title,
    string? Notes,
    DateTimeOffset? DueAt,
    Guid? OwnerEmployeeId = null);
