using Platform.Domain.Entities;

namespace Platform.Application.Opportunities;

internal static class OpportunityStageResponseMapper
{
    internal static OpportunityStageResponse ToResponse(OpportunityStage stage)
    {
        return new OpportunityStageResponse(
            stage.Id,
            stage.TenantId,
            stage.Name,
            stage.Position,
            stage.IsActive,
            stage.CreatedAt,
            stage.UpdatedAt);
    }
}
