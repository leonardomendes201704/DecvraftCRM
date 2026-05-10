using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Customers;

public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerResponse?>
{
    private readonly ICustomerService _customerService;

    public GetCustomerByIdQueryHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public Task<CustomerResponse?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        return _customerService.GetByIdAsync(request.TenantId, request.CustomerId, cancellationToken);
    }
}
