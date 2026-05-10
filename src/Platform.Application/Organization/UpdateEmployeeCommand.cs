using MediatR;

namespace Platform.Application.Organization;

public sealed record UpdateEmployeeCommand(Guid TenantId, Guid EmployeeId, UpdateEmployeeRequest Request)
    : IRequest<EmployeeOperationResult>;
