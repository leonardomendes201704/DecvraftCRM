using MediatR;

namespace Platform.Application.Opportunities;

public sealed record DeactivateOpportunityStageCommand(Guid TenantId, Guid StageId)
    : IRequest<OpportunityStageOperationResult>;
