using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class ListUpcomingOpportunityActivitiesQueryHandler
    : IRequestHandler<ListUpcomingOpportunityActivitiesQuery, IReadOnlyCollection<OpportunityActivityResponse>>
{
    private const int DefaultDays = 7;
    private const int MaxDays = 90;

    private readonly IOpportunityActivityRepository _activityRepository;
    private readonly IClock _clock;

    public ListUpcomingOpportunityActivitiesQueryHandler(
        IOpportunityActivityRepository activityRepository,
        IClock clock)
    {
        _activityRepository = activityRepository;
        _clock = clock;
    }

    public async Task<IReadOnlyCollection<OpportunityActivityResponse>> Handle(
        ListUpcomingOpportunityActivitiesQuery request,
        CancellationToken cancellationToken)
    {
        var days = request.Days <= 0 ? DefaultDays : Math.Min(request.Days, MaxDays);
        var from = _clock.UtcNow;
        var to = from.AddDays(days);

        var activities = await _activityRepository.ListUpcomingAsync(
            request.TenantId,
            from,
            to,
            request.OwnerEmployeeId,
            cancellationToken);

        return activities.Select(OpportunityActivityResponseMapper.ToResponse).ToArray();
    }
}
