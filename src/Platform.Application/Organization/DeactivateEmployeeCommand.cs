using MediatR;

namespace Platform.Application.Organization;

public sealed record DeactivateEmployeeCommand(Guid TenantId, Guid EmployeeId)
    : IRequest<EmployeeOperationResult>;
