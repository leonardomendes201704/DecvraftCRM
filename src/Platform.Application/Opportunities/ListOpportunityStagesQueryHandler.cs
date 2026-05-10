using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class ListOpportunityStagesQueryHandler
    : IRequestHandler<ListOpportunityStagesQuery, IReadOnlyCollection<OpportunityStageResponse>>
{
    private readonly IOpportunityStageRepository _stageRepository;

    public ListOpportunityStagesQueryHandler(IOpportunityStageRepository stageRepository)
    {
        _stageRepository = stageRepository;
    }

    public async Task<IReadOnlyCollection<OpportunityStageResponse>> Handle(
        ListOpportunityStagesQuery request,
        CancellationToken cancellationToken)
    {
        var stages = await _stageRepository.ListAsync(request.TenantId, cancellationToken);

        return stages.Select(OpportunityStageResponseMapper.ToResponse).ToArray();
    }
}
