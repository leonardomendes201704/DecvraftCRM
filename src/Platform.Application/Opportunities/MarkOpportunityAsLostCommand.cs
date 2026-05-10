using MediatR;

namespace Platform.Application.Opportunities;

public sealed record MarkOpportunityAsLostCommand(Guid TenantId, Guid OpportunityId)
    : IRequest<OpportunityOperationResult>;
