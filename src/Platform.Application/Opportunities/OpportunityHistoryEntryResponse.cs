using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed record OpportunityHistoryEntryResponse(
    Guid Id,
    Guid TenantId,
    Guid OpportunityId,
    OpportunityHistoryEventType Type,
    string Description,
    DateTimeOffset CreatedAt);
