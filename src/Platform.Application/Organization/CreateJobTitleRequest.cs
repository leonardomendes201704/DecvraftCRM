namespace Platform.Application.Organization;

public sealed record CreateJobTitleRequest(string Name, string Code, int Level, bool IsLeadership);
