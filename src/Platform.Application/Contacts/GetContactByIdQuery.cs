using MediatR;

namespace Platform.Application.Contacts;

public sealed record GetContactByIdQuery(Guid TenantId, Guid ContactId) : IRequest<ContactResponse?>;
