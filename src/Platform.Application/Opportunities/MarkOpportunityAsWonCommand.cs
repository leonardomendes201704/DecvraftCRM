using MediatR;

namespace Platform.Application.Opportunities;

public sealed record MarkOpportunityAsWonCommand(Guid TenantId, Guid OpportunityId)
    : IRequest<OpportunityOperationResult>;
