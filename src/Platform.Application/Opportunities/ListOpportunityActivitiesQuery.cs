using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListOpportunityActivitiesQuery(Guid TenantId, Guid OpportunityId)
    : IRequest<IReadOnlyCollection<OpportunityActivityResponse>>;
