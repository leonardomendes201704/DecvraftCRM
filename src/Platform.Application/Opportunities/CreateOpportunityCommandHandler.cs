using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed class CreateOpportunityCommandHandler
    : IRequestHandler<CreateOpportunityCommand, OpportunityOperationResult>
{
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IOpportunityStageRepository _stageRepository;
    private readonly IOpportunityHistoryRepository _historyRepository;
    private readonly IClock _clock;

    public CreateOpportunityCommandHandler(
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
        CreateOpportunityCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Title) || request.Request.EstimatedValue < decimal.Zero)
        {
            return OpportunityOperationResult.InvalidInput();
        }

        var customerExists = await _opportunityRepository.CustomerExistsAsync(
            request.TenantId,
            request.CustomerId,
            cancellationToken);

        if (!customerExists)
        {
            return OpportunityOperationResult.CustomerNotFound();
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

        if (request.Request.OwnerEmployeeId is not null)
        {
            var ownerExists = await _opportunityRepository.ActiveEmployeeExistsAsync(
                request.TenantId,
                request.Request.OwnerEmployeeId.Value,
                cancellationToken);

            if (!ownerExists)
            {
                return OpportunityOperationResult.OwnerNotFound();
            }
        }

        var now = _clock.UtcNow;
        var opportunity = Opportunity.Create(
            request.TenantId,
            request.CustomerId,
            request.Request.Title,
            request.Request.EstimatedValue,
            request.Request.ExpectedCloseDate,
            now,
            request.Request.StageId,
            request.Request.OwnerEmployeeId);

        _opportunityRepository.Add(opportunity);
        _historyRepository.Add(OpportunityHistoryEntry.Create(
            request.TenantId,
            opportunity.Id,
            OpportunityHistoryEventType.Created,
            $"Oportunidade criada: {opportunity.Title}.",
            now));
        await _opportunityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(OpportunityResponseMapper.ToResponse(opportunity));
    }
}
