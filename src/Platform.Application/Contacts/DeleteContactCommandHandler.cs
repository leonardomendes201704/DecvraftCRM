using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Contacts;

public sealed class DeleteContactCommandHandler
    : IRequestHandler<DeleteContactCommand, ContactOperationResult>
{
    private readonly IContactService _contactService;

    public DeleteContactCommandHandler(IContactService contactService)
    {
        _contactService = contactService;
    }

    public Task<ContactOperationResult> Handle(
        DeleteContactCommand request,
        CancellationToken cancellationToken)
    {
        return _contactService.DeleteAsync(request.TenantId, request.ContactId, cancellationToken);
    }
}
