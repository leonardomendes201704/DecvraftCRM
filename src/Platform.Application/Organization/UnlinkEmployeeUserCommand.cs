using MediatR;

namespace Platform.Application.Organization;

public sealed record UnlinkEmployeeUserCommand(Guid TenantId, Guid EmployeeId)
    : IRequest<EmployeeLinkOperationResult>;
