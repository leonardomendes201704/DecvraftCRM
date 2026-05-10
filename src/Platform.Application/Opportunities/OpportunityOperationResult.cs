using Platform.Application.Common;
using Platform.Domain.Enums;

namespace Platform.Application.Opportunities;

public sealed record OpportunityOperationResult(
    OpportunityOperationStatus Status,
    OpportunityResponse? Opportunity) : IApplicationOperationResult<OpportunityOperationStatus>
{
    public bool Succeeded => Status == OpportunityOperationStatus.Success;

    public static OpportunityOperationResult Success(OpportunityResponse opportunity) =>
        new(OpportunityOperationStatus.Success, opportunity);

    public static OpportunityOperationResult NotFound() =>
        new(OpportunityOperationStatus.NotFound, null);

    public static OpportunityOperationResult CustomerNotFound() =>
        new(OpportunityOperationStatus.CustomerNotFound, null);

    public static OpportunityOperationResult StageNotFound() =>
        new(OpportunityOperationStatus.StageNotFound, null);

    public static OpportunityOperationResult InvalidInput() =>
        new(OpportunityOperationStatus.InvalidInput, null);
}
