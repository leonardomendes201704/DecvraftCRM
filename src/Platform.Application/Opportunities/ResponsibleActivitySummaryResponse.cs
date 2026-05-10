namespace Platform.Application.Opportunities;

public sealed record ResponsibleActivitySummaryResponse(
    Guid? OwnerEmployeeId,
    string OwnerName,
    int ActivityCount);
