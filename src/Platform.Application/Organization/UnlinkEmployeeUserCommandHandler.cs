using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class UnlinkEmployeeUserCommandHandler
    : IRequestHandler<UnlinkEmployeeUserCommand, EmployeeLinkOperationResult>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IClock _clock;

    public UnlinkEmployeeUserCommandHandler(IEmployeeRepository employeeRepository, IClock clock)
    {
        _employeeRepository = employeeRepository;
        _clock = clock;
    }

    public async Task<EmployeeLinkOperationResult> Handle(
        UnlinkEmployeeUserCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(
            request.TenantId,
            request.EmployeeId,
            cancellationToken);

        if (employee is null)
        {
            return EmployeeLinkOperationResult.EmployeeNotFound();
        }

        employee.UnlinkUser(_clock.UtcNow);
        await _employeeRepository.SaveChangesAsync(cancellationToken);

        return EmployeeLinkOperationResult.Success(EmployeeResponseMapper.ToResponse(employee));
    }
}
