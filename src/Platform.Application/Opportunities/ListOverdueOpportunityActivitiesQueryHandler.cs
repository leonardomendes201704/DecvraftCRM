using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class ListOverdueOpportunityActivitiesQueryHandler
    : IRequestHandler<ListOverdueOpportunityActivitiesQuery, IReadOnlyCollection<OpportunityActivityResponse>>
{
    private readonly IOpportunityActivityRepository _activityRepository;
    private readonly IClock _clock;

    public ListOverdueOpportunityActivitiesQueryHandler(
        IOpportunityActivityRepository activityRepository,
        IClock clock)
    {
        _activityRepository = activityRepository;
        _clock = clock;
    }

    public async Task<IReadOnlyCollection<OpportunityActivityResponse>> Handle(
        ListOverdueOpportunityActivitiesQuery request,
        CancellationToken cancellationToken)
    {
        var activities = await _activityRepository.ListOverdueAsync(
            request.TenantId,
            _clock.UtcNow,
            cancellationToken);

        return activities.Select(OpportunityActivityResponseMapper.ToResponse).ToArray();
    }
}
