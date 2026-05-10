using MediatR;

namespace Platform.Application.Customers;

public sealed record CreateCustomerCommand(Guid TenantId, CreateCustomerRequest Request)
    : IRequest<CustomerOperationResult>;
