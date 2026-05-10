using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Customers;

public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerResponse?>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponse?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(
            request.TenantId,
            request.CustomerId,
            cancellationToken);

        return customer is null ? null : CustomerResponseMapper.ToResponse(customer);
    }
}
