using MediatR;

namespace Platform.Application.Customers;

public sealed record UpdateCustomerCommand(
    Guid TenantId,
    Guid CustomerId,
    UpdateCustomerRequest Request) : IRequest<CustomerOperationResult>;
