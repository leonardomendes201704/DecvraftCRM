using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed class UpdateOpportunityActivityCommandHandler
    : IRequestHandler<UpdateOpportunityActivityCommand, OpportunityActivityOperationResult>
{
    private readonly IOpportunityActivityRepository _activityRepository;
    private readonly IOpportunityHistoryRepository _historyRepository;
    private readonly IClock _clock;

    public UpdateOpportunityActivityCommandHandler(
        IOpportunityActivityRepository activityRepository,
        IOpportunityHistoryRepository historyRepository,
        IClock clock)
    {
        _activityRepository = activityRepository;
        _historyRepository = historyRepository;
        _clock = clock;
    }

    public async Task<OpportunityActivityOperationResult> Handle(
        UpdateOpportunityActivityCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Title))
        {
            return OpportunityActivityOperationResult.InvalidInput();
        }

        var activity = await _activityRepository.GetByIdAsync(
            request.TenantId,
            request.ActivityId,
            cancellationToken);

        if (activity is null)
        {
            return OpportunityActivityOperationResult.NotFound();
        }

        var now = _clock.UtcNow;
        activity.Update(
            request.Request.Type,
            request.Request.Title,
            request.Request.Notes,
            request.Request.DueAt,
            now);

        _historyRepository.Add(OpportunityHistoryEntry.Create(
            request.TenantId,
            activity.OpportunityId,
            OpportunityHistoryEventType.ActivityUpdated,
            $"Atividade atualizada: {activity.Title}.",
            now));

        await _activityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityActivityOperationResult.Success(OpportunityActivityResponseMapper.ToResponse(activity));
    }
}
