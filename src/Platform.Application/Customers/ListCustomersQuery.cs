using MediatR;

namespace Platform.Application.Customers;

public sealed record ListCustomersQuery(Guid TenantId) : IRequest<IReadOnlyCollection<CustomerResponse>>;
