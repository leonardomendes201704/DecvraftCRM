using Platform.Domain.Entities;

namespace Platform.Application.Opportunities;

internal static class OpportunityResponseMapper
{
    internal static OpportunityResponse ToResponse(Opportunity opportunity)
    {
        return new OpportunityResponse(
            opportunity.Id,
            opportunity.TenantId,
            opportunity.CustomerId,
            opportunity.Title,
            opportunity.EstimatedValue,
            opportunity.ExpectedCloseDate,
            opportunity.Status,
            opportunity.CreatedAt,
            opportunity.UpdatedAt);
    }
}
