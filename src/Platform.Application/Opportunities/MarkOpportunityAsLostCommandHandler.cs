using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed class MarkOpportunityAsLostCommandHandler
    : IRequestHandler<MarkOpportunityAsLostCommand, OpportunityOperationResult>
{
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IOpportunityHistoryRepository _historyRepository;
    private readonly IClock _clock;

    public MarkOpportunityAsLostCommandHandler(
        IOpportunityRepository opportunityRepository,
        IOpportunityHistoryRepository historyRepository,
        IClock clock)
    {
        _opportunityRepository = opportunityRepository;
        _historyRepository = historyRepository;
        _clock = clock;
    }

    public async Task<OpportunityOperationResult> Handle(
        MarkOpportunityAsLostCommand request,
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
        opportunity.MarkAsLost(now);
        _historyRepository.Add(OpportunityHistoryEntry.Create(
            request.TenantId,
            opportunity.Id,
            OpportunityHistoryEventType.Lost,
            $"Oportunidade perdida: {opportunity.Title}.",
            now));
        await _opportunityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(OpportunityResponseMapper.ToResponse(opportunity));
    }
}
