using MediatR;

namespace Platform.Application.Customers;

public sealed record GetCustomerByIdQuery(Guid TenantId, Guid CustomerId) : IRequest<CustomerResponse?>;
