namespace Platform.Application.Organization;

public sealed record UpdateJobTitleRequest(string Name, string Code, int Level, bool IsLeadership);
