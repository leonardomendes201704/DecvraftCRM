using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;

namespace Platform.Application.Organization;

public sealed class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, EmployeeOperationResult>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IClock _clock;

    public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IClock clock)
    {
        _employeeRepository = employeeRepository;
        _clock = clock;
    }

    public async Task<EmployeeOperationResult> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.FullName))
        {
            return EmployeeOperationResult.InvalidInput();
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
                cancellationToken: cancellationToken);

            if (exists)
            {
                return EmployeeOperationResult.DuplicateEmail();
            }
        }

        var employee = Employee.Create(
            request.TenantId,
            null,
            request.Request.DepartmentId,
            request.Request.JobTitleId,
            request.Request.ManagerEmployeeId,
            request.Request.FullName,
            request.Request.Document,
            request.Request.CorporateEmail,
            request.Request.Phone,
            request.Request.HireDate,
            _clock.UtcNow);

        _employeeRepository.Add(employee);
        await _employeeRepository.SaveChangesAsync(cancellationToken);

        return EmployeeOperationResult.Success(EmployeeResponseMapper.ToResponse(employee));
    }

    private async Task<bool> RelatedEntitiesExistAsync(
        Guid tenantId,
        CreateEmployeeRequest request,
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
