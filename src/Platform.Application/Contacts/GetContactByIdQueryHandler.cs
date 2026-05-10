using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Contacts;

public sealed class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactResponse?>
{
    private readonly IContactRepository _contactRepository;

    public GetContactByIdQueryHandler(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<ContactResponse?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository.GetByIdAsync(request.TenantId, request.ContactId, cancellationToken);

        return contact is null ? null : ContactResponseMapper.ToResponse(contact);
    }
}
