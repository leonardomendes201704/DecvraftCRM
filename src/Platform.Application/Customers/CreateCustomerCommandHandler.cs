using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;

namespace Platform.Application.Customers;

public sealed class CreateCustomerCommandHandler
    : IRequestHandler<CreateCustomerCommand, CustomerOperationResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IClock clock)
    {
        _customerRepository = customerRepository;
        _clock = clock;
    }

    public async Task<CustomerOperationResult> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Name) || string.IsNullOrWhiteSpace(request.Request.Document))
        {
            return CustomerOperationResult.InvalidInput();
        }

        var document = request.Request.Document.Trim();
        var exists = await _customerRepository.DocumentExistsAsync(
            request.TenantId,
            document,
            null,
            cancellationToken);

        if (exists)
        {
            return CustomerOperationResult.DuplicateDocument();
        }

        var customer = Customer.Create(
            request.TenantId,
            request.Request.Name,
            document,
            request.Request.Type,
            _clock.UtcNow);

        _customerRepository.Add(customer);
        await _customerRepository.SaveChangesAsync(cancellationToken);

        return CustomerOperationResult.Success(CustomerResponseMapper.ToResponse(customer));
    }
}
