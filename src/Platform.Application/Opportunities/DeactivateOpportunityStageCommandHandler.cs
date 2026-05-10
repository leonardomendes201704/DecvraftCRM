using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class DeactivateOpportunityStageCommandHandler
    : IRequestHandler<DeactivateOpportunityStageCommand, OpportunityStageOperationResult>
{
    private readonly IOpportunityStageRepository _stageRepository;
    private readonly IClock _clock;

    public DeactivateOpportunityStageCommandHandler(IOpportunityStageRepository stageRepository, IClock clock)
    {
        _stageRepository = stageRepository;
        _clock = clock;
    }

    public async Task<OpportunityStageOperationResult> Handle(
        DeactivateOpportunityStageCommand request,
        CancellationToken cancellationToken)
    {
        var stage = await _stageRepository.GetByIdAsync(request.TenantId, request.StageId, cancellationToken);
        if (stage is null)
        {
            return OpportunityStageOperationResult.NotFound();
        }

        stage.Deactivate(_clock.UtcNow);
        await _stageRepository.SaveChangesAsync(cancellationToken);

        return OpportunityStageOperationResult.Success(OpportunityStageResponseMapper.ToResponse(stage));
    }
}
