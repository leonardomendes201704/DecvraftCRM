using Platform.Domain.Entities;

namespace Platform.Application.Opportunities;

internal static class OpportunityHistoryEntryResponseMapper
{
    internal static OpportunityHistoryEntryResponse ToResponse(OpportunityHistoryEntry entry)
    {
        return new OpportunityHistoryEntryResponse(
            entry.Id,
            entry.TenantId,
            entry.OpportunityId,
            entry.Type,
            entry.Description,
            entry.CreatedAt);
    }
}
