using Platform.Application.Common;
using Platform.Domain.Enums;

namespace Platform.Application.Organization;

public sealed record DepartmentOperationResult(
    OrganizationOperationStatus Status,
    DepartmentResponse? Department) : IApplicationOperationResult<OrganizationOperationStatus>
{
    public bool Succeeded => Status == OrganizationOperationStatus.Success;

    public static DepartmentOperationResult Success(DepartmentResponse department) =>
        new(OrganizationOperationStatus.Success, department);

    public static DepartmentOperationResult NotFound() => new(OrganizationOperationStatus.NotFound, null);

    public static DepartmentOperationResult DuplicateCode() => new(OrganizationOperationStatus.DuplicateCode, null);

    public static DepartmentOperationResult InvalidInput() => new(OrganizationOperationStatus.InvalidInput, null);
}
