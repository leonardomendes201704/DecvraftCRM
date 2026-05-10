using MediatR;

namespace Platform.Application.Contacts;

public sealed record ListContactsByCustomerQuery(Guid TenantId, Guid CustomerId)
    : IRequest<IReadOnlyCollection<ContactResponse>>;
