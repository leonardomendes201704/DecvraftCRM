using MediatR;

namespace Platform.Application.Organization;

public sealed record LinkEmployeeUserCommand(
    Guid TenantId,
    Guid EmployeeId,
    LinkEmployeeUserRequest Request) : IRequest<EmployeeLinkOperationResult>;
