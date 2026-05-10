using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed class CompleteOpportunityActivityCommandHandler
    : IRequestHandler<CompleteOpportunityActivityCommand, OpportunityActivityOperationResult>
{
    private readonly IOpportunityActivityRepository _activityRepository;
    private readonly IOpportunityHistoryRepository _historyRepository;
    private readonly IClock _clock;

    public CompleteOpportunityActivityCommandHandler(
        IOpportunityActivityRepository activityRepository,
        IOpportunityHistoryRepository historyRepository,
        IClock clock)
    {
        _activityRepository = activityRepository;
        _historyRepository = historyRepository;
        _clock = clock;
    }

    public async Task<OpportunityActivityOperationResult> Handle(
        CompleteOpportunityActivityCommand request,
        CancellationToken cancellationToken)
    {
        var activity = await _activityRepository.GetByIdAsync(request.TenantId, request.ActivityId, cancellationToken);
        if (activity is null)
        {
            return OpportunityActivityOperationResult.NotFound();
        }

        var now = _clock.UtcNow;
        activity.Complete(now);
        _historyRepository.Add(OpportunityHistoryEntry.Create(
            request.TenantId,
            activity.OpportunityId,
            OpportunityHistoryEventType.ActivityCompleted,
            $"Atividade concluida: {activity.Title}.",
            now));

        await _activityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityActivityOperationResult.Success(OpportunityActivityResponseMapper.ToResponse(activity));
    }
}
