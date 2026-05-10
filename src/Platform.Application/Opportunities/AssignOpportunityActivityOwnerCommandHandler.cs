using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed class AssignOpportunityActivityOwnerCommandHandler
    : IRequestHandler<AssignOpportunityActivityOwnerCommand, OpportunityActivityOperationResult>
{
    private readonly IOpportunityActivityRepository _activityRepository;
    private readonly IOpportunityHistoryRepository _historyRepository;
    private readonly IClock _clock;

    public AssignOpportunityActivityOwnerCommandHandler(
        IOpportunityActivityRepository activityRepository,
        IOpportunityHistoryRepository historyRepository,
        IClock clock)
    {
        _activityRepository = activityRepository;
        _historyRepository = historyRepository;
        _clock = clock;
    }

    public async Task<OpportunityActivityOperationResult> Handle(
        AssignOpportunityActivityOwnerCommand request,
        CancellationToken cancellationToken)
    {
        var activity = await _activityRepository.GetByIdAsync(
            request.TenantId,
            request.ActivityId,
            cancellationToken);

        if (activity is null)
        {
            return OpportunityActivityOperationResult.NotFound();
        }

        if (request.Request.OwnerEmployeeId is not null)
        {
            var ownerExists = await _activityRepository.ActiveEmployeeExistsAsync(
                request.TenantId,
                request.Request.OwnerEmployeeId.Value,
                cancellationToken);

            if (!ownerExists)
            {
                return OpportunityActivityOperationResult.OwnerNotFound();
            }
        }

        var now = _clock.UtcNow;
        activity.AssignOwner(request.Request.OwnerEmployeeId, now);
        _historyRepository.Add(OpportunityHistoryEntry.Create(
            request.TenantId,
            activity.OpportunityId,
            OpportunityHistoryEventType.ActivityOwnerChanged,
            request.Request.OwnerEmployeeId is null
                ? $"Responsavel da atividade removido: {activity.Title}."
                : $"Responsavel da atividade alterado: {activity.Title}.",
            now));

        await _activityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityActivityOperationResult.Success(OpportunityActivityResponseMapper.ToResponse(activity));
    }
}
