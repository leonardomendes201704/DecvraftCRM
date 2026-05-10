using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class ListEmployeesQueryHandler
    : IRequestHandler<ListEmployeesQuery, IReadOnlyCollection<EmployeeResponse>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public ListEmployeesQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IReadOnlyCollection<EmployeeResponse>> Handle(
        ListEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.ListAsync(request.TenantId, cancellationToken);

        return employees.Select(EmployeeResponseMapper.ToResponse).ToArray();
    }
}
