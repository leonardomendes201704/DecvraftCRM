using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class ListOpportunityActivitiesQueryHandler
    : IRequestHandler<ListOpportunityActivitiesQuery, IReadOnlyCollection<OpportunityActivityResponse>>
{
    private readonly IOpportunityActivityRepository _activityRepository;

    public ListOpportunityActivitiesQueryHandler(IOpportunityActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<IReadOnlyCollection<OpportunityActivityResponse>> Handle(
        ListOpportunityActivitiesQuery request,
        CancellationToken cancellationToken)
    {
        var activities = await _activityRepository.ListByOpportunityAsync(
            request.TenantId,
            request.OpportunityId,
            cancellationToken);

        return activities.Select(OpportunityActivityResponseMapper.ToResponse).ToArray();
    }
}
