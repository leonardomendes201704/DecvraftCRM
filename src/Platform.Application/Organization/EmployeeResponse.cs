using Platform.Domain.Enums;

namespace Platform.Application.Organization;

public sealed record EmployeeResponse(
    Guid Id,
    Guid TenantId,
    Guid? ApplicationUserId,
    Guid? DepartmentId,
    Guid? JobTitleId,
    Guid? ManagerEmployeeId,
    string FullName,
    string? Document,
    string? CorporateEmail,
    string? Phone,
    DateOnly? HireDate,
    DateOnly? TerminationDate,
    EmployeeStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
