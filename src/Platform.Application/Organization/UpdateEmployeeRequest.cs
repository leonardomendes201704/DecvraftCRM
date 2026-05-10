namespace Platform.Application.Organization;

public sealed record UpdateEmployeeRequest(
    Guid? DepartmentId,
    Guid? JobTitleId,
    Guid? ManagerEmployeeId,
    string FullName,
    string? Document,
    string? CorporateEmail,
    string? Phone,
    DateOnly? HireDate);
