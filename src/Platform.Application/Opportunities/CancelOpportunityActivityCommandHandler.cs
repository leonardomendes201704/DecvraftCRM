using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed class CancelOpportunityActivityCommandHandler
    : IRequestHandler<CancelOpportunityActivityCommand, OpportunityActivityOperationResult>
{
    private readonly IOpportunityActivityRepository _activityRepository;
    private readonly IOpportunityHistoryRepository _historyRepository;
    private readonly IClock _clock;

    public CancelOpportunityActivityCommandHandler(
        IOpportunityActivityRepository activityRepository,
        IOpportunityHistoryRepository historyRepository,
        IClock clock)
    {
        _activityRepository = activityRepository;
        _historyRepository = historyRepository;
        _clock = clock;
    }

    public async Task<OpportunityActivityOperationResult> Handle(
        CancelOpportunityActivityCommand request,
        CancellationToken cancellationToken)
    {
        var activity = await _activityRepository.GetByIdAsync(request.TenantId, request.ActivityId, cancellationToken);
        if (activity is null)
        {
            return OpportunityActivityOperationResult.NotFound();
        }

        var now = _clock.UtcNow;
        activity.Cancel(now);
        _historyRepository.Add(OpportunityHistoryEntry.Create(
            request.TenantId,
            activity.OpportunityId,
            OpportunityHistoryEventType.ActivityCanceled,
            $"Atividade cancelada: {activity.Title}.",
            now));

        await _activityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityActivityOperationResult.Success(OpportunityActivityResponseMapper.ToResponse(activity));
    }
}
