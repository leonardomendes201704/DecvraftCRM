using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Enums;

namespace Platform.Application.Organization;

public sealed class DeactivateEmployeeCommandHandler
    : IRequestHandler<DeactivateEmployeeCommand, EmployeeOperationResult>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IClock _clock;

    public DeactivateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IClock clock)
    {
        _employeeRepository = employeeRepository;
        _clock = clock;
    }

    public async Task<EmployeeOperationResult> Handle(
        DeactivateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(
            request.TenantId,
            request.EmployeeId,
            cancellationToken);

        if (employee is null)
        {
            return EmployeeOperationResult.NotFound();
        }

        employee.ChangeStatus(EmployeeStatus.Inactive, _clock.UtcNow);
        await _employeeRepository.SaveChangesAsync(cancellationToken);

        return EmployeeOperationResult.Success(EmployeeResponseMapper.ToResponse(employee));
    }
}
