using MediatR;

namespace Platform.Application.Opportunities;

public sealed record CreateOpportunityStageCommand(
    Guid TenantId,
    CreateOpportunityStageRequest Request) : IRequest<OpportunityStageOperationResult>;
