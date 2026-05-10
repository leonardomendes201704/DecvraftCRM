using MediatR;

namespace Platform.Application.Contacts;

public sealed record DeleteContactCommand(Guid TenantId, Guid ContactId)
    : IRequest<ContactOperationResult>;
