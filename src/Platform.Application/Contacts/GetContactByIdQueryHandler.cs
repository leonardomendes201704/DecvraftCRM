using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Contacts;

public sealed class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactResponse?>
{
    private readonly IContactService _contactService;

    public GetContactByIdQueryHandler(IContactService contactService)
    {
        _contactService = contactService;
    }

    public Task<ContactResponse?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
    {
        return _contactService.GetByIdAsync(request.TenantId, request.ContactId, cancellationToken);
    }
}
