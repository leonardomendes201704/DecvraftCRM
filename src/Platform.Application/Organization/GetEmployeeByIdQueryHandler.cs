using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeResponse?>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeResponse?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(
            request.TenantId,
            request.EmployeeId,
            cancellationToken);

        return employee is null ? null : EmployeeResponseMapper.ToResponse(employee);
    }
}
