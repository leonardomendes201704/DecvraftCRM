using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;

namespace Platform.Application.Opportunities;

public sealed class CreateOpportunityStageCommandHandler
    : IRequestHandler<CreateOpportunityStageCommand, OpportunityStageOperationResult>
{
    private readonly IOpportunityStageRepository _stageRepository;
    private readonly IClock _clock;

    public CreateOpportunityStageCommandHandler(IOpportunityStageRepository stageRepository, IClock clock)
    {
        _stageRepository = stageRepository;
        _clock = clock;
    }

    public async Task<OpportunityStageOperationResult> Handle(
        CreateOpportunityStageCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Name) || request.Request.Position <= 0)
        {
            return OpportunityStageOperationResult.InvalidInput();
        }

        var exists = await _stageRepository.ExistsByNameAsync(
            request.TenantId,
            request.Request.Name,
            cancellationToken: cancellationToken);

        if (exists)
        {
            return OpportunityStageOperationResult.DuplicateName();
        }

        var stage = OpportunityStage.Create(
            request.TenantId,
            request.Request.Name,
            request.Request.Position,
            _clock.UtcNow);

        _stageRepository.Add(stage);
        await _stageRepository.SaveChangesAsync(cancellationToken);

        return OpportunityStageOperationResult.Success(OpportunityStageResponseMapper.ToResponse(stage));
    }
}
