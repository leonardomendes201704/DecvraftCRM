using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed class MoveOpportunityStageCommandHandler
    : IRequestHandler<MoveOpportunityStageCommand, OpportunityOperationResult>
{
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IOpportunityStageRepository _stageRepository;
    private readonly IOpportunityHistoryRepository _historyRepository;
    private readonly IClock _clock;

    public MoveOpportunityStageCommandHandler(
        IOpportunityRepository opportunityRepository,
        IOpportunityStageRepository stageRepository,
        IOpportunityHistoryRepository historyRepository,
        IClock clock)
    {
        _opportunityRepository = opportunityRepository;
        _stageRepository = stageRepository;
        _historyRepository = historyRepository;
        _clock = clock;
    }

    public async Task<OpportunityOperationResult> Handle(
        MoveOpportunityStageCommand request,
        CancellationToken cancellationToken)
    {
        var opportunity = await _opportunityRepository.GetByIdAsync(
            request.TenantId,
            request.OpportunityId,
            cancellationToken);

        if (opportunity is null)
        {
            return OpportunityOperationResult.NotFound();
        }

        var stage = await _stageRepository.GetByIdAsync(request.TenantId, request.Request.StageId, cancellationToken);
        if (stage is null || !stage.IsActive)
        {
            return OpportunityOperationResult.StageNotFound();
        }

        var now = _clock.UtcNow;
        opportunity.MoveToStage(stage.Id, now);
        _historyRepository.Add(OpportunityHistoryEntry.Create(
            request.TenantId,
            opportunity.Id,
            OpportunityHistoryEventType.StageChanged,
            $"Oportunidade movida para a etapa {stage.Name}.",
            now));

        await _opportunityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(OpportunityResponseMapper.ToResponse(opportunity));
    }
}
