namespace Platform.Application.Opportunities;

public sealed record ResponsiblePortfolioSummaryResponse(
    Guid? OwnerEmployeeId,
    string OwnerName,
    int OpenOpportunities,
    int WonOpportunities,
    int LostOpportunities,
    int CanceledOpportunities,
    decimal OpenEstimatedValue);
