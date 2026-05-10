using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class ListResponsiblePortfolioSummaryQueryHandler
    : IRequestHandler<ListResponsiblePortfolioSummaryQuery, IReadOnlyCollection<ResponsiblePortfolioSummaryResponse>>
{
    private readonly IOpportunityRepository _opportunityRepository;

    public ListResponsiblePortfolioSummaryQueryHandler(IOpportunityRepository opportunityRepository)
    {
        _opportunityRepository = opportunityRepository;
    }

    public Task<IReadOnlyCollection<ResponsiblePortfolioSummaryResponse>> Handle(
        ListResponsiblePortfolioSummaryQuery request,
        CancellationToken cancellationToken)
    {
        return _opportunityRepository.ListResponsiblePortfolioSummaryAsync(request.TenantId, cancellationToken);
    }
}
