using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Contacts;

public sealed class UpdateContactCommandHandler
    : IRequestHandler<UpdateContactCommand, ContactOperationResult>
{
    private readonly IContactService _contactService;

    public UpdateContactCommandHandler(IContactService contactService)
    {
        _contactService = contactService;
    }

    public Task<ContactOperationResult> Handle(
        UpdateContactCommand request,
        CancellationToken cancellationToken)
    {
        return _contactService.UpdateAsync(
            request.TenantId,
            request.ContactId,
            request.Request,
            cancellationToken);
    }
}
