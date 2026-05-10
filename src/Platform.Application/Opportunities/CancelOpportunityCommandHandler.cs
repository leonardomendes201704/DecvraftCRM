using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed class CancelOpportunityCommandHandler
    : IRequestHandler<CancelOpportunityCommand, OpportunityOperationResult>
{
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IOpportunityHistoryRepository _historyRepository;
    private readonly IClock _clock;

    public CancelOpportunityCommandHandler(
        IOpportunityRepository opportunityRepository,
        IOpportunityHistoryRepository historyRepository,
        IClock clock)
    {
        _opportunityRepository = opportunityRepository;
        _historyRepository = historyRepository;
        _clock = clock;
    }

    public async Task<OpportunityOperationResult> Handle(
        CancelOpportunityCommand request,
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

        var now = _clock.UtcNow;
        opportunity.Cancel(now);
        _historyRepository.Add(OpportunityHistoryEntry.Create(
            request.TenantId,
            opportunity.Id,
            OpportunityHistoryEventType.Canceled,
            $"Oportunidade cancelada: {opportunity.Title}.",
            now));
        await _opportunityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(OpportunityResponseMapper.ToResponse(opportunity));
    }
}
