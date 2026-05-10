using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class ListDepartmentsQueryHandler
    : IRequestHandler<ListDepartmentsQuery, IReadOnlyCollection<DepartmentResponse>>
{
    private readonly IDepartmentRepository _departmentRepository;

    public ListDepartmentsQueryHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IReadOnlyCollection<DepartmentResponse>> Handle(
        ListDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        var departments = await _departmentRepository.ListAsync(request.TenantId, cancellationToken);

        return departments.Select(DepartmentResponseMapper.ToResponse).ToArray();
    }
}
