using Platform.Domain.Entities;

namespace Platform.Application.Organization;

internal static class EmployeeResponseMapper
{
    internal static EmployeeResponse ToResponse(Employee employee)
    {
        return new EmployeeResponse(
            employee.Id,
            employee.TenantId,
            employee.ApplicationUserId,
            employee.DepartmentId,
            employee.JobTitleId,
            employee.ManagerEmployeeId,
            employee.FullName,
            employee.Document,
            employee.CorporateEmail,
            employee.Phone,
            employee.HireDate,
            employee.TerminationDate,
            employee.Status,
            employee.CreatedAt,
            employee.UpdatedAt);
    }
}
