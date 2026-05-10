using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Customers;

public sealed class ListCustomersQueryHandler
    : IRequestHandler<ListCustomersQuery, IReadOnlyCollection<CustomerResponse>>
{
    private readonly ICustomerRepository _customerRepository;

    public ListCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IReadOnlyCollection<CustomerResponse>> Handle(
        ListCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.ListAsync(request.TenantId, cancellationToken);

        return customers
            .Select(CustomerResponseMapper.ToResponse)
            .ToArray();
    }
}
