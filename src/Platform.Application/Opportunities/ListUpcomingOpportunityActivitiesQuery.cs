using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListUpcomingOpportunityActivitiesQuery(Guid TenantId, int Days)
    : IRequest<IReadOnlyCollection<OpportunityActivityResponse>>;
