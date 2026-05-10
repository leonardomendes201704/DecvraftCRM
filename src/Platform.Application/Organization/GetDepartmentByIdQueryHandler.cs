using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentResponse?>
{
    private readonly IDepartmentRepository _departmentRepository;

    public GetDepartmentByIdQueryHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<DepartmentResponse?> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await _departmentRepository.GetByIdAsync(
            request.TenantId,
            request.DepartmentId,
            cancellationToken);

        return department is null ? null : DepartmentResponseMapper.ToResponse(department);
    }
}
