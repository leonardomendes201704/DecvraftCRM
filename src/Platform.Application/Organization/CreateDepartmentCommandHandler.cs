using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;

namespace Platform.Application.Organization;

public sealed class CreateDepartmentCommandHandler
    : IRequestHandler<CreateDepartmentCommand, DepartmentOperationResult>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IClock _clock;

    public CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository, IClock clock)
    {
        _departmentRepository = departmentRepository;
        _clock = clock;
    }

    public async Task<DepartmentOperationResult> Handle(
        CreateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Name) || string.IsNullOrWhiteSpace(request.Request.Code))
        {
            return DepartmentOperationResult.InvalidInput();
        }

        var exists = await _departmentRepository.ExistsByCodeAsync(
            request.TenantId,
            request.Request.Code,
            cancellationToken: cancellationToken);

        if (exists)
        {
            return DepartmentOperationResult.DuplicateCode();
        }

        var department = Department.Create(
            request.TenantId,
            request.Request.Name,
            request.Request.Code,
            _clock.UtcNow);

        _departmentRepository.Add(department);
        await _departmentRepository.SaveChangesAsync(cancellationToken);

        return DepartmentOperationResult.Success(DepartmentResponseMapper.ToResponse(department));
    }
}
