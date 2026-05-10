namespace Platform.Application.Opportunities;

public sealed record ResponsibleOpportunitySummaryResponse(
    Guid? OwnerEmployeeId,
    string OwnerName,
    int TotalOpportunities,
    int OpenOpportunities,
    decimal OpenEstimatedValue);
