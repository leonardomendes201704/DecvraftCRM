using Platform.Domain.Enums;

namespace Platform.Application.Customers;

public sealed record CustomerOperationResult(
    CustomerOperationStatus Status,
    CustomerResponse? Customer)
{
    public static CustomerOperationResult Success(CustomerResponse customer) =>
        new(CustomerOperationStatus.Success, customer);

    public static CustomerOperationResult NotFound() =>
        new(CustomerOperationStatus.NotFound, null);

    public static CustomerOperationResult DuplicateDocument() =>
        new(CustomerOperationStatus.DuplicateDocument, null);

    public static CustomerOperationResult InvalidInput() =>
        new(CustomerOperationStatus.InvalidInput, null);
}
