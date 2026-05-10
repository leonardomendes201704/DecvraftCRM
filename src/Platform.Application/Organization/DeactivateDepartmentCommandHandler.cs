using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class DeactivateDepartmentCommandHandler
    : IRequestHandler<DeactivateDepartmentCommand, DepartmentOperationResult>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IClock _clock;

    public DeactivateDepartmentCommandHandler(IDepartmentRepository departmentRepository, IClock clock)
    {
        _departmentRepository = departmentRepository;
        _clock = clock;
    }

    public async Task<DepartmentOperationResult> Handle(
        DeactivateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var department = await _departmentRepository.GetByIdAsync(
            request.TenantId,
            request.DepartmentId,
            cancellationToken);

        if (department is null)
        {
            return DepartmentOperationResult.NotFound();
        }

        department.Deactivate(_clock.UtcNow);
        await _departmentRepository.SaveChangesAsync(cancellationToken);

        return DepartmentOperationResult.Success(DepartmentResponseMapper.ToResponse(department));
    }
}
