namespace Platform.Application.Opportunities;

public sealed record UpdateOpportunityRequest(
    string Title,
    decimal EstimatedValue,
    DateOnly? ExpectedCloseDate,
    Guid? StageId = null);
