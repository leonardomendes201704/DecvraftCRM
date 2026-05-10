using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed record UpdateOpportunityActivityRequest(
    OpportunityActivityType Type,
    string Title,
    string? Notes,
    DateTimeOffset? DueAt);
