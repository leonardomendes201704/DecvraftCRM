using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class ListResponsibleOverdueActivitySummaryQueryHandler
    : IRequestHandler<ListResponsibleOverdueActivitySummaryQuery, IReadOnlyCollection<ResponsibleActivitySummaryResponse>>
{
    private readonly IOpportunityActivityRepository _activityRepository;
    private readonly IClock _clock;

    public ListResponsibleOverdueActivitySummaryQueryHandler(
        IOpportunityActivityRepository activityRepository,
        IClock clock)
    {
        _activityRepository = activityRepository;
        _clock = clock;
    }

    public Task<IReadOnlyCollection<ResponsibleActivitySummaryResponse>> Handle(
        ListResponsibleOverdueActivitySummaryQuery request,
        CancellationToken cancellationToken)
    {
        return _activityRepository.ListOverdueSummaryByResponsibleAsync(
            request.TenantId,
            _clock.UtcNow,
            cancellationToken);
    }
}
