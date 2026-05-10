using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Contacts;

public sealed class ListContactsByCustomerQueryHandler
    : IRequestHandler<ListContactsByCustomerQuery, IReadOnlyCollection<ContactResponse>>
{
    private readonly IContactRepository _contactRepository;

    public ListContactsByCustomerQueryHandler(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<IReadOnlyCollection<ContactResponse>> Handle(
        ListContactsByCustomerQuery request,
        CancellationToken cancellationToken)
    {
        var customerExists = await _contactRepository.CustomerExistsAsync(
            request.TenantId,
            request.CustomerId,
            cancellationToken);

        if (!customerExists)
        {
            return [];
        }

        var contacts = await _contactRepository.ListByCustomerAsync(
            request.TenantId,
            request.CustomerId,
            cancellationToken);

        return contacts
            .Select(ContactResponseMapper.ToResponse)
            .ToArray();
    }
}
