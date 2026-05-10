using Platform.Domain.Entities;

namespace Platform.Application.Organization;

internal static class DepartmentResponseMapper
{
    internal static DepartmentResponse ToResponse(Department department)
    {
        return new DepartmentResponse(
            department.Id,
            department.TenantId,
            department.Name,
            department.Code,
            department.IsActive,
            department.CreatedAt,
            department.UpdatedAt);
    }
}
