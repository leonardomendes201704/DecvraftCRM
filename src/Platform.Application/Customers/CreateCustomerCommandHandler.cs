using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Customers;

public sealed class CreateCustomerCommandHandler
    : IRequestHandler<CreateCustomerCommand, CustomerOperationResult>
{
    private readonly ICustomerService _customerService;

    public CreateCustomerCommandHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public Task<CustomerOperationResult> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        return _customerService.CreateAsync(request.TenantId, request.Request, cancellationToken);
    }
}
