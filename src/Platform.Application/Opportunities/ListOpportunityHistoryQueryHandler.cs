using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class ListOpportunityHistoryQueryHandler
    : IRequestHandler<ListOpportunityHistoryQuery, IReadOnlyCollection<OpportunityHistoryEntryResponse>>
{
    private readonly IOpportunityHistoryRepository _historyRepository;

    public ListOpportunityHistoryQueryHandler(IOpportunityHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    public async Task<IReadOnlyCollection<OpportunityHistoryEntryResponse>> Handle(
        ListOpportunityHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var entries = await _historyRepository.ListByOpportunityAsync(
            request.TenantId,
            request.OpportunityId,
            cancellationToken);

        return entries.Select(OpportunityHistoryEntryResponseMapper.ToResponse).ToArray();
    }
}
