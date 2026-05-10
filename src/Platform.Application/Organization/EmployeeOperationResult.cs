using Platform.Application.Common;
using Platform.Domain.Enums;

namespace Platform.Application.Organization;

public sealed record EmployeeOperationResult(
    OrganizationOperationStatus Status,
    EmployeeResponse? Employee) : IApplicationOperationResult<OrganizationOperationStatus>
{
    public bool Succeeded => Status == OrganizationOperationStatus.Success;

    public static EmployeeOperationResult Success(EmployeeResponse employee) =>
        new(OrganizationOperationStatus.Success, employee);

    public static EmployeeOperationResult NotFound() => new(OrganizationOperationStatus.NotFound, null);

    public static EmployeeOperationResult DuplicateEmail() => new(OrganizationOperationStatus.DuplicateEmail, null);

    public static EmployeeOperationResult InvalidInput() => new(OrganizationOperationStatus.InvalidInput, null);

    public static EmployeeOperationResult RelatedEntityNotFound() =>
        new(OrganizationOperationStatus.RelatedEntityNotFound, null);
}
