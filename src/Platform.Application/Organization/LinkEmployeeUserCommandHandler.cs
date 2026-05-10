using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class LinkEmployeeUserCommandHandler
    : IRequestHandler<LinkEmployeeUserCommand, EmployeeLinkOperationResult>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IClock _clock;

    public LinkEmployeeUserCommandHandler(IEmployeeRepository employeeRepository, IClock clock)
    {
        _employeeRepository = employeeRepository;
        _clock = clock;
    }

    public async Task<EmployeeLinkOperationResult> Handle(
        LinkEmployeeUserCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Request.ApplicationUserId == Guid.Empty)
        {
            return EmployeeLinkOperationResult.InvalidInput();
        }

        var employee = await _employeeRepository.GetByIdAsync(
            request.TenantId,
            request.EmployeeId,
            cancellationToken);

        if (employee is null)
        {
            return EmployeeLinkOperationResult.EmployeeNotFound();
        }

        var userExists = await _employeeRepository.ApplicationUserExistsAsync(
            request.TenantId,
            request.Request.ApplicationUserId,
            cancellationToken);

        if (!userExists)
        {
            return EmployeeLinkOperationResult.UserNotFound();
        }

        var userAlreadyLinked = await _employeeRepository.ApplicationUserLinkedAsync(
            request.TenantId,
            request.Request.ApplicationUserId,
            employee.Id,
            cancellationToken);

        if (userAlreadyLinked)
        {
            return EmployeeLinkOperationResult.UserAlreadyLinked();
        }

        employee.LinkUser(request.Request.ApplicationUserId, _clock.UtcNow);
        await _employeeRepository.SaveChangesAsync(cancellationToken);

        return EmployeeLinkOperationResult.Success(EmployeeResponseMapper.ToResponse(employee));
    }
}
