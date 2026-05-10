using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListOverdueOpportunityActivitiesQuery(Guid TenantId)
    : IRequest<IReadOnlyCollection<OpportunityActivityResponse>>;
