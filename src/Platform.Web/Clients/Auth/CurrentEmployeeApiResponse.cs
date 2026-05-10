namespace Platform.Web.Clients.Auth;

public sealed record CurrentEmployeeApiResponse(
    Guid Id,
    Guid? DepartmentId,
    Guid? JobTitleId,
    Guid? ManagerEmployeeId,
    string FullName,
    string? CorporateEmail,
    int Status);
