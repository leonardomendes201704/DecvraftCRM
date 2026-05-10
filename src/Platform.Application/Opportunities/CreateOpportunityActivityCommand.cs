using MediatR;

namespace Platform.Application.Opportunities;

public sealed record CreateOpportunityActivityCommand(
    Guid TenantId,
    Guid OpportunityId,
    CreateOpportunityActivityRequest Request) : IRequest<OpportunityActivityOperationResult>;
