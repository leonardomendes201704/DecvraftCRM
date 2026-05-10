using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class ListTeamFunnelSummaryQueryHandler
    : IRequestHandler<ListTeamFunnelSummaryQuery, IReadOnlyCollection<TeamFunnelSummaryResponse>>
{
    private readonly IOpportunityRepository _opportunityRepository;

    public ListTeamFunnelSummaryQueryHandler(IOpportunityRepository opportunityRepository)
    {
        _opportunityRepository = opportunityRepository;
    }

    public Task<IReadOnlyCollection<TeamFunnelSummaryResponse>> Handle(
        ListTeamFunnelSummaryQuery request,
        CancellationToken cancellationToken)
    {
        return _opportunityRepository.ListTeamFunnelSummaryAsync(request.TenantId, cancellationToken);
    }
}
