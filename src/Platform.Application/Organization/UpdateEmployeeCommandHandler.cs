using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class UpdateEmployeeCommandHandler
    : IRequestHandler<UpdateEmployeeCommand, EmployeeOperationResult>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IClock _clock;

    public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IClock clock)
    {
        _employeeRepository = employeeRepository;
        _clock = clock;
    }

    public async Task<EmployeeOperationResult> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.FullName)
            || request.Request.ManagerEmployeeId == request.EmployeeId)
        {
            return EmployeeOperationResult.InvalidInput();
        }

        var employee = await _employeeRepository.GetByIdAsync(
            request.TenantId,
            request.EmployeeId,
            cancellationToken);

        if (employee is null)
        {
            return EmployeeOperationResult.NotFound();
        }

        if (!await RelatedEntitiesExistAsync(request.TenantId, request.Request, cancellationToken))
        {
            return EmployeeOperationResult.RelatedEntityNotFound();
        }

        if (!string.IsNullOrWhiteSpace(request.Request.CorporateEmail))
        {
            var exists = await _employeeRepository.ExistsByCorporateEmailAsync(
                request.TenantId,
                request.Request.CorporateEmail,
                request.EmployeeId,
                cancellationToken);

            if (exists)
            {
                return EmployeeOperationResult.DuplicateEmail();
            }
        }

        employee.UpdateProfile(
            request.Request.DepartmentId,
            request.Request.JobTitleId,
            request.Request.ManagerEmployeeId,
            request.Request.FullName,
            request.Request.Document,
            request.Request.CorporateEmail,
            request.Request.Phone,
            request.Request.HireDate,
            _clock.UtcNow);
        await _employeeRepository.SaveChangesAsync(cancellationToken);

        return EmployeeOperationResult.Success(EmployeeResponseMapper.ToResponse(employee));
    }

    private async Task<bool> RelatedEntitiesExistAsync(
        Guid tenantId,
        UpdateEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        if (request.DepartmentId is not null
            && !await _employeeRepository.DepartmentExistsAsync(tenantId, request.DepartmentId.Value, cancellationToken))
        {
            return false;
        }

        if (request.JobTitleId is not null
            && !await _employeeRepository.JobTitleExistsAsync(tenantId, request.JobTitleId.Value, cancellationToken))
        {
            return false;
        }

        return request.ManagerEmployeeId is null
            || await _employeeRepository.EmployeeExistsAsync(tenantId, request.ManagerEmployeeId.Value, cancellationToken);
    }
}
