using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Customers;

public sealed class DeactivateCustomerCommandHandler
    : IRequestHandler<DeactivateCustomerCommand, CustomerOperationResult>
{
    private readonly ICustomerService _customerService;

    public DeactivateCustomerCommandHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public Task<CustomerOperationResult> Handle(
        DeactivateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        return _customerService.DeactivateAsync(request.TenantId, request.CustomerId, cancellationToken);
    }
}
