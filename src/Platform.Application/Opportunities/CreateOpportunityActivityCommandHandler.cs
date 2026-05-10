using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed class CreateOpportunityActivityCommandHandler
    : IRequestHandler<CreateOpportunityActivityCommand, OpportunityActivityOperationResult>
{
    private readonly IOpportunityActivityRepository _activityRepository;
    private readonly IOpportunityHistoryRepository _historyRepository;
    private readonly IClock _clock;

    public CreateOpportunityActivityCommandHandler(
        IOpportunityActivityRepository activityRepository,
        IOpportunityHistoryRepository historyRepository,
        IClock clock)
    {
        _activityRepository = activityRepository;
        _historyRepository = historyRepository;
        _clock = clock;
    }

    public async Task<OpportunityActivityOperationResult> Handle(
        CreateOpportunityActivityCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Title))
        {
            return OpportunityActivityOperationResult.InvalidInput();
        }

        var opportunityExists = await _activityRepository.OpportunityExistsAsync(
            request.TenantId,
            request.OpportunityId,
            cancellationToken);

        if (!opportunityExists)
        {
            return OpportunityActivityOperationResult.OpportunityNotFound();
        }

        var now = _clock.UtcNow;
        var activity = OpportunityActivity.Create(
            request.TenantId,
            request.OpportunityId,
            request.Request.Type,
            request.Request.Title,
            request.Request.Notes,
            request.Request.DueAt,
            now);

        _activityRepository.Add(activity);
        _historyRepository.Add(OpportunityHistoryEntry.Create(
            request.TenantId,
            request.OpportunityId,
            OpportunityHistoryEventType.ActivityCreated,
            $"Atividade criada: {activity.Title}.",
            now));

        await _activityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityActivityOperationResult.Success(OpportunityActivityResponseMapper.ToResponse(activity));
    }
}
