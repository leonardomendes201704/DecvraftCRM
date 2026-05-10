using Platform.Application.Common;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed record OpportunityStageOperationResult(
    OpportunityStageOperationStatus Status,
    OpportunityStageResponse? Stage) : IApplicationOperationResult<OpportunityStageOperationStatus>
{
    public bool Succeeded => Status == OpportunityStageOperationStatus.Success;

    public static OpportunityStageOperationResult Success(OpportunityStageResponse stage) =>
        new(OpportunityStageOperationStatus.Success, stage);

    public static OpportunityStageOperationResult NotFound() =>
        new(OpportunityStageOperationStatus.NotFound, null);

    public static OpportunityStageOperationResult DuplicateName() =>
        new(OpportunityStageOperationStatus.DuplicateName, null);

    public static OpportunityStageOperationResult InvalidInput() =>
        new(OpportunityStageOperationStatus.InvalidInput, null);
}
