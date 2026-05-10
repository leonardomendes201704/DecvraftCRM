using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class ListResponsibleUpcomingActivitySummaryQueryHandler
    : IRequestHandler<ListResponsibleUpcomingActivitySummaryQuery, IReadOnlyCollection<ResponsibleActivitySummaryResponse>>
{
    private const int DefaultDays = 7;
    private const int MaxDays = 90;

    private readonly IOpportunityActivityRepository _activityRepository;
    private readonly IClock _clock;

    public ListResponsibleUpcomingActivitySummaryQueryHandler(
        IOpportunityActivityRepository activityRepository,
        IClock clock)
    {
        _activityRepository = activityRepository;
        _clock = clock;
    }

    public Task<IReadOnlyCollection<ResponsibleActivitySummaryResponse>> Handle(
        ListResponsibleUpcomingActivitySummaryQuery request,
        CancellationToken cancellationToken)
    {
        var days = request.Days <= 0 ? DefaultDays : Math.Min(request.Days, MaxDays);
        var from = _clock.UtcNow;
        var to = from.AddDays(days);

        return _activityRepository.ListUpcomingSummaryByResponsibleAsync(
            request.TenantId,
            from,
            to,
            cancellationToken);
    }
}
