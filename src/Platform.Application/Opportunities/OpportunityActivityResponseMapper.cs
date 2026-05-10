using Platform.Domain.Entities;

namespace Platform.Application.Opportunities;

internal static class OpportunityActivityResponseMapper
{
    internal static OpportunityActivityResponse ToResponse(OpportunityActivity activity)
    {
        return new OpportunityActivityResponse(
            activity.Id,
            activity.TenantId,
            activity.OpportunityId,
            activity.Type,
            activity.Title,
            activity.Notes,
            activity.DueAt,
            activity.Status,
            activity.CompletedAt,
            activity.CreatedAt,
            activity.UpdatedAt);
    }
}
