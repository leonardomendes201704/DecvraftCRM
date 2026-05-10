using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Customers;

public sealed class UpdateCustomerCommandHandler
    : IRequestHandler<UpdateCustomerCommand, CustomerOperationResult>
{
    private readonly ICustomerService _customerService;

    public UpdateCustomerCommandHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public Task<CustomerOperationResult> Handle(
        UpdateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        return _customerService.UpdateAsync(
            request.TenantId,
            request.CustomerId,
            request.Request,
            cancellationToken);
    }
}
