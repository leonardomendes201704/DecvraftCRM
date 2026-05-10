using MediatR;

namespace Platform.Application.Opportunities;

public sealed record UpdateOpportunityCommand(
    Guid TenantId,
    Guid OpportunityId,
    UpdateOpportunityRequest Request) : IRequest<OpportunityOperationResult>;
