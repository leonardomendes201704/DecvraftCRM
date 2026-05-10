using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class UpdateOpportunityStageCommandHandler
    : IRequestHandler<UpdateOpportunityStageCommand, OpportunityStageOperationResult>
{
    private readonly IOpportunityStageRepository _stageRepository;
    private readonly IClock _clock;

    public UpdateOpportunityStageCommandHandler(IOpportunityStageRepository stageRepository, IClock clock)
    {
        _stageRepository = stageRepository;
        _clock = clock;
    }

    public async Task<OpportunityStageOperationResult> Handle(
        UpdateOpportunityStageCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Name) || request.Request.Position <= 0)
        {
            return OpportunityStageOperationResult.InvalidInput();
        }

        var stage = await _stageRepository.GetByIdAsync(request.TenantId, request.StageId, cancellationToken);
        if (stage is null)
        {
            return OpportunityStageOperationResult.NotFound();
        }

        var exists = await _stageRepository.ExistsByNameAsync(
            request.TenantId,
            request.Request.Name,
            request.StageId,
            cancellationToken);

        if (exists)
        {
            return OpportunityStageOperationResult.DuplicateName();
        }

        stage.Update(request.Request.Name, request.Request.Position, _clock.UtcNow);
        await _stageRepository.SaveChangesAsync(cancellationToken);

        return OpportunityStageOperationResult.Success(OpportunityStageResponseMapper.ToResponse(stage));
    }
}
