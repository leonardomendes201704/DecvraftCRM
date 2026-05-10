using MediatR;

namespace Platform.Application.Organization;

public sealed record CreateEmployeeCommand(Guid TenantId, CreateEmployeeRequest Request)
    : IRequest<EmployeeOperationResult>;
