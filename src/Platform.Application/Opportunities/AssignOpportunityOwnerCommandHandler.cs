using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed class AssignOpportunityOwnerCommandHandler
    : IRequestHandler<AssignOpportunityOwnerCommand, OpportunityOperationResult>
{
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IOpportunityHistoryRepository _historyRepository;
    private readonly IClock _clock;

    public AssignOpportunityOwnerCommandHandler(
        IOpportunityRepository opportunityRepository,
        IOpportunityHistoryRepository historyRepository,
        IClock clock)
    {
        _opportunityRepository = opportunityRepository;
        _historyRepository = historyRepository;
        _clock = clock;
    }

    public async Task<OpportunityOperationResult> Handle(
        AssignOpportunityOwnerCommand request,
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
        opportunity.AssignOwner(request.Request.OwnerEmployeeId, now);
        _historyRepository.Add(OpportunityHistoryEntry.Create(
            request.TenantId,
            opportunity.Id,
            OpportunityHistoryEventType.OwnerChanged,
            request.Request.OwnerEmployeeId is null
                ? "Responsavel da oportunidade removido."
                : "Responsavel da oportunidade alterado.",
            now));

        await _opportunityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(OpportunityResponseMapper.ToResponse(opportunity));
    }
}
