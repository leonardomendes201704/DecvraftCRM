using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListOverdueOpportunityActivitiesQuery(Guid TenantId, Guid? OwnerEmployeeId = null)
    : IRequest<IReadOnlyCollection<OpportunityActivityResponse>>;
