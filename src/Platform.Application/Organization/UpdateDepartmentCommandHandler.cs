using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class UpdateDepartmentCommandHandler
    : IRequestHandler<UpdateDepartmentCommand, DepartmentOperationResult>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IClock _clock;

    public UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository, IClock clock)
    {
        _departmentRepository = departmentRepository;
        _clock = clock;
    }

    public async Task<DepartmentOperationResult> Handle(
        UpdateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Name) || string.IsNullOrWhiteSpace(request.Request.Code))
        {
            return DepartmentOperationResult.InvalidInput();
        }

        var department = await _departmentRepository.GetByIdAsync(
            request.TenantId,
            request.DepartmentId,
            cancellationToken);

        if (department is null)
        {
            return DepartmentOperationResult.NotFound();
        }

        var exists = await _departmentRepository.ExistsByCodeAsync(
            request.TenantId,
            request.Request.Code,
            request.DepartmentId,
            cancellationToken);

        if (exists)
        {
            return DepartmentOperationResult.DuplicateCode();
        }

        department.Update(request.Request.Name, request.Request.Code, _clock.UtcNow);
        await _departmentRepository.SaveChangesAsync(cancellationToken);

        return DepartmentOperationResult.Success(DepartmentResponseMapper.ToResponse(department));
    }
}
