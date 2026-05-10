using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Contacts;

public sealed class CreateContactCommandHandler
    : IRequestHandler<CreateContactCommand, ContactOperationResult>
{
    private readonly IContactService _contactService;

    public CreateContactCommandHandler(IContactService contactService)
    {
        _contactService = contactService;
    }

    public Task<ContactOperationResult> Handle(
        CreateContactCommand request,
        CancellationToken cancellationToken)
    {
        return _contactService.CreateAsync(
            request.TenantId,
            request.CustomerId,
            request.Request,
            cancellationToken);
    }
}
