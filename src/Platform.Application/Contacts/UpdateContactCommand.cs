using MediatR;

namespace Platform.Application.Contacts;

public sealed record UpdateContactCommand(
    Guid TenantId,
    Guid ContactId,
    UpdateContactRequest Request) : IRequest<ContactOperationResult>;
