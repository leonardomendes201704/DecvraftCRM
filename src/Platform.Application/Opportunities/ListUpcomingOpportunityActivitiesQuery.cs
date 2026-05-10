using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListUpcomingOpportunityActivitiesQuery(Guid TenantId, int Days, Guid? OwnerEmployeeId = null)
    : IRequest<IReadOnlyCollection<OpportunityActivityResponse>>;
