using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed class UpdateOpportunityCommandHandler
    : IRequestHandler<UpdateOpportunityCommand, OpportunityOperationResult>
{
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IOpportunityStageRepository _stageRepository;
    private readonly IOpportunityHistoryRepository _historyRepository;
    private readonly IClock _clock;

    public UpdateOpportunityCommandHandler(
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
        UpdateOpportunityCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Title) || request.Request.EstimatedValue < decimal.Zero)
        {
            return OpportunityOperationResult.InvalidInput();
        }

        var opportunity = await _opportunityRepository.GetByIdAsync(
            request.TenantId,
            request.OpportunityId,
            cancellationToken);

        if (opportunity is null)
        {
            return OpportunityOperationResult.NotFound();
        }

        if (request.Request.StageId is not null)
        {
            var stage = await _stageRepository.GetByIdAsync(
                request.TenantId,
                request.Request.StageId.Value,
                cancellationToken);

            if (stage is null || !stage.IsActive)
            {
                return OpportunityOperationResult.StageNotFound();
            }
        }

        var now = _clock.UtcNow;
        opportunity.Update(
            request.Request.Title,
            request.Request.EstimatedValue,
            request.Request.ExpectedCloseDate,
            now,
            request.Request.StageId);
        _historyRepository.Add(OpportunityHistoryEntry.Create(
            request.TenantId,
            opportunity.Id,
            OpportunityHistoryEventType.Updated,
            $"Oportunidade atualizada: {opportunity.Title}.",
            now));
        await _opportunityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(OpportunityResponseMapper.ToResponse(opportunity));
    }
}
