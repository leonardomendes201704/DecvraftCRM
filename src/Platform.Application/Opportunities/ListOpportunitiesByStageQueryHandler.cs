using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class ListOpportunitiesByStageQueryHandler
    : IRequestHandler<ListOpportunitiesByStageQuery, IReadOnlyCollection<OpportunityResponse>>
{
    private readonly IOpportunityRepository _opportunityRepository;

    public ListOpportunitiesByStageQueryHandler(IOpportunityRepository opportunityRepository)
    {
        _opportunityRepository = opportunityRepository;
    }

    public async Task<IReadOnlyCollection<OpportunityResponse>> Handle(
        ListOpportunitiesByStageQuery request,
        CancellationToken cancellationToken)
    {
        var opportunities = await _opportunityRepository.ListByStageAsync(
            request.TenantId,
            request.StageId,
            cancellationToken);

        return opportunities.Select(OpportunityResponseMapper.ToResponse).ToArray();
    }
}
