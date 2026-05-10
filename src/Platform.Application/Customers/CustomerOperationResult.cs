using Platform.Application.Common;
using Platform.Domain.Enums;

namespace Platform.Application.Customers;

public sealed record CustomerOperationResult(
    CustomerOperationStatus Status,
    CustomerResponse? Customer) : IApplicationOperationResult<CustomerOperationStatus>
{
    public bool Succeeded => Status == CustomerOperationStatus.Success;

    public static CustomerOperationResult Success(CustomerResponse customer) =>
        new(CustomerOperationStatus.Success, customer);

    public static CustomerOperationResult NotFound() =>
        new(CustomerOperationStatus.NotFound, null);

    public static CustomerOperationResult DuplicateDocument() =>
        new(CustomerOperationStatus.DuplicateDocument, null);

    public static CustomerOperationResult InvalidInput() =>
        new(CustomerOperationStatus.InvalidInput, null);
}
