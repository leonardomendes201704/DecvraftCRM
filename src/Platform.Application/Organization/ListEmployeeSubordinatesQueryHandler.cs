using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class ListEmployeeSubordinatesQueryHandler
    : IRequestHandler<ListEmployeeSubordinatesQuery, IReadOnlyCollection<EmployeeResponse>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public ListEmployeeSubordinatesQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IReadOnlyCollection<EmployeeResponse>> Handle(
        ListEmployeeSubordinatesQuery request,
        CancellationToken cancellationToken)
    {
        var subordinates = await _employeeRepository.ListSubordinatesAsync(
            request.TenantId,
            request.EmployeeId,
            cancellationToken);

        return subordinates.Select(EmployeeResponseMapper.ToResponse).ToArray();
    }
}
