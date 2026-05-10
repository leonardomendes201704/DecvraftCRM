using MediatR;

namespace Platform.Application.Opportunities;

public sealed record MoveOpportunityStageCommand(
    Guid TenantId,
    Guid OpportunityId,
    MoveOpportunityStageRequest Request) : IRequest<OpportunityOperationResult>;
