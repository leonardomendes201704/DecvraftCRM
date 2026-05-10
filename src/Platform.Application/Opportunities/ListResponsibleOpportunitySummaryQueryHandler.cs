using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class ListResponsibleOpportunitySummaryQueryHandler
    : IRequestHandler<ListResponsibleOpportunitySummaryQuery, IReadOnlyCollection<ResponsibleOpportunitySummaryResponse>>
{
    private readonly IOpportunityRepository _opportunityRepository;

    public ListResponsibleOpportunitySummaryQueryHandler(IOpportunityRepository opportunityRepository)
    {
        _opportunityRepository = opportunityRepository;
    }

    public Task<IReadOnlyCollection<ResponsibleOpportunitySummaryResponse>> Handle(
        ListResponsibleOpportunitySummaryQuery request,
        CancellationToken cancellationToken)
    {
        return _opportunityRepository.ListResponsibleOpportunitySummaryAsync(request.TenantId, cancellationToken);
    }
}
