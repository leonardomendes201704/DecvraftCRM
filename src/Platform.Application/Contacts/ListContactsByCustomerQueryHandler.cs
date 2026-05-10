using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Contacts;

public sealed class ListContactsByCustomerQueryHandler
    : IRequestHandler<ListContactsByCustomerQuery, IReadOnlyCollection<ContactResponse>>
{
    private readonly IContactService _contactService;

    public ListContactsByCustomerQueryHandler(IContactService contactService)
    {
        _contactService = contactService;
    }

    public Task<IReadOnlyCollection<ContactResponse>> Handle(
        ListContactsByCustomerQuery request,
        CancellationToken cancellationToken)
    {
        return _contactService.ListByCustomerAsync(
            request.TenantId,
            request.CustomerId,
            cancellationToken);
    }
}
