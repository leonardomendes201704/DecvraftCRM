using MediatR;

namespace Platform.Application.Contacts;

public sealed record CreateContactCommand(
    Guid TenantId,
    Guid CustomerId,
    CreateContactRequest Request) : IRequest<ContactOperationResult>;
