using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Customers;

public sealed class ListCustomersQueryHandler
    : IRequestHandler<ListCustomersQuery, IReadOnlyCollection<CustomerResponse>>
{
    private readonly ICustomerService _customerService;

    public ListCustomersQueryHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public Task<IReadOnlyCollection<CustomerResponse>> Handle(
        ListCustomersQuery request,
        CancellationToken cancellationToken)
    {
        return _customerService.ListAsync(request.TenantId, cancellationToken);
    }
}
