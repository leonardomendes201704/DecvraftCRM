namespace Platform.Application.Opportunities;

public sealed record CreateOpportunityRequest(
    string Title,
    decimal EstimatedValue,
    DateOnly? ExpectedCloseDate,
    Guid? StageId = null);
