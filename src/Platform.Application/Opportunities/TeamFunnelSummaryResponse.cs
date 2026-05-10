namespace Platform.Application.Opportunities;

public sealed record TeamFunnelSummaryResponse(
    Guid? DepartmentId,
    string DepartmentName,
    int OpenOpportunities,
    int WonOpportunities,
    int LostOpportunities,
    int CanceledOpportunities,
    decimal OpenEstimatedValue);
