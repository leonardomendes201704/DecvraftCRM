using MediatR;

namespace Platform.Application.Opportunities;

public sealed record GetOpportunityByIdQuery(Guid TenantId, Guid OpportunityId)
    : IRequest<OpportunityResponse?>;
