using Platform.Application.Common;
using Platform.Domain.Enums;

namespace Platform.Application.Organization;

public sealed record EmployeeLinkOperationResult(
    EmployeeLinkOperationStatus Status,
    EmployeeResponse? Employee) : IApplicationOperationResult<EmployeeLinkOperationStatus>
{
    public bool Succeeded => Status == EmployeeLinkOperationStatus.Success;

    public static EmployeeLinkOperationResult Success(EmployeeResponse employee) =>
        new(EmployeeLinkOperationStatus.Success, employee);

    public static EmployeeLinkOperationResult EmployeeNotFound() =>
        new(EmployeeLinkOperationStatus.EmployeeNotFound, null);

    public static EmployeeLinkOperationResult UserNotFound() =>
        new(EmployeeLinkOperationStatus.UserNotFound, null);

    public static EmployeeLinkOperationResult UserAlreadyLinked() =>
        new(EmployeeLinkOperationStatus.UserAlreadyLinked, null);

    public static EmployeeLinkOperationResult InvalidInput() =>
        new(EmployeeLinkOperationStatus.InvalidInput, null);
}
