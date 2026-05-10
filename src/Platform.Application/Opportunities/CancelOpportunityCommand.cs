using MediatR;

namespace Platform.Application.Opportunities;

public sealed record CancelOpportunityCommand(Guid TenantId, Guid OpportunityId)
    : IRequest<OpportunityOperationResult>;
