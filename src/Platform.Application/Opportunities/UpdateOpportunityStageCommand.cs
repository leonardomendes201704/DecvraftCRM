using MediatR;

namespace Platform.Application.Opportunities;

public sealed record UpdateOpportunityStageCommand(
    Guid TenantId,
    Guid StageId,
    UpdateOpportunityStageRequest Request) : IRequest<OpportunityStageOperationResult>;
