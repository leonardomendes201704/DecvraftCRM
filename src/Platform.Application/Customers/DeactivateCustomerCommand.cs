using MediatR;

namespace Platform.Application.Customers;

public sealed record DeactivateCustomerCommand(Guid TenantId, Guid CustomerId)
    : IRequest<CustomerOperationResult>;
