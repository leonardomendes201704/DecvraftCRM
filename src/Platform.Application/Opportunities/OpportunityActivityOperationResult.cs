using Platform.Application.Common;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed record OpportunityActivityOperationResult(
    OpportunityActivityOperationStatus Status,
    OpportunityActivityResponse? Activity) : IApplicationOperationResult<OpportunityActivityOperationStatus>
{
    public bool Succeeded => Status == OpportunityActivityOperationStatus.Success;

    public static OpportunityActivityOperationResult Success(OpportunityActivityResponse activity) =>
        new(OpportunityActivityOperationStatus.Success, activity);

    public static OpportunityActivityOperationResult NotFound() =>
        new(OpportunityActivityOperationStatus.NotFound, null);

    public static OpportunityActivityOperationResult OpportunityNotFound() =>
        new(OpportunityActivityOperationStatus.OpportunityNotFound, null);

    public static OpportunityActivityOperationResult InvalidInput() =>
        new(OpportunityActivityOperationStatus.InvalidInput, null);
}
