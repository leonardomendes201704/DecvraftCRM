using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Customers;

public sealed class DeactivateCustomerCommandHandler
    : IRequestHandler<DeactivateCustomerCommand, CustomerOperationResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;

    public DeactivateCustomerCommandHandler(ICustomerRepository customerRepository, IClock clock)
    {
        _customerRepository = customerRepository;
        _clock = clock;
    }

    public async Task<CustomerOperationResult> Handle(
        DeactivateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(
            request.TenantId,
            request.CustomerId,
            cancellationToken);

        if (customer is null)
        {
            return CustomerOperationResult.NotFound();
        }

        customer.Deactivate(_clock.UtcNow);
        await _customerRepository.SaveChangesAsync(cancellationToken);

        return CustomerOperationResult.Success(CustomerResponseMapper.ToResponse(customer));
    }
}
