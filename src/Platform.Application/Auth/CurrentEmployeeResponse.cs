using Platform.Domain.Enums;

namespace Platform.Application.Auth;

public sealed record CurrentEmployeeResponse(
    Guid Id,
    Guid? DepartmentId,
    Guid? JobTitleId,
    Guid? ManagerEmployeeId,
    string FullName,
    string? CorporateEmail,
    EmployeeStatus Status);
