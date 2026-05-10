using Platform.Application.Common;
using Platform.Domain.Enums;

namespace Platform.Application.Contacts;

public sealed record ContactOperationResult(
    ContactOperationStatus Status,
    ContactResponse? Contact) : IApplicationOperationResult<ContactOperationStatus>
{
    public bool Succeeded => Status == ContactOperationStatus.Success;

    public static ContactOperationResult Success(ContactResponse contact) =>
        new(ContactOperationStatus.Success, contact);

    public static ContactOperationResult NotFound() =>
        new(ContactOperationStatus.NotFound, null);

    public static ContactOperationResult CustomerNotFound() =>
        new(ContactOperationStatus.CustomerNotFound, null);

    public static ContactOperationResult DuplicateEmail() =>
        new(ContactOperationStatus.DuplicateEmail, null);

    public static ContactOperationResult InvalidInput() =>
        new(ContactOperationStatus.InvalidInput, null);
}
