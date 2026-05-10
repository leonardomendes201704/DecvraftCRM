using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Customers;

public sealed class UpdateCustomerCommandHandler
    : IRequestHandler<UpdateCustomerCommand, CustomerOperationResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IClock clock)
    {
        _customerRepository = customerRepository;
        _clock = clock;
    }

    public async Task<CustomerOperationResult> Handle(
        UpdateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Name) || string.IsNullOrWhiteSpace(request.Request.Document))
        {
            return CustomerOperationResult.InvalidInput();
        }

        var customer = await _customerRepository.GetByIdAsync(
            request.TenantId,
            request.CustomerId,
            cancellationToken);

        if (customer is null)
        {
            return CustomerOperationResult.NotFound();
        }

        var document = request.Request.Document.Trim();
        var documentInUse = await _customerRepository.DocumentExistsAsync(
            request.TenantId,
            document,
            request.CustomerId,
            cancellationToken);

        if (documentInUse)
        {
            return CustomerOperationResult.DuplicateDocument();
        }

        customer.Update(request.Request.Name, document, request.Request.Type, _clock.UtcNow);
        await _customerRepository.SaveChangesAsync(cancellationToken);

        return CustomerOperationResult.Success(CustomerResponseMapper.ToResponse(customer));
    }
}
