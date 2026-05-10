using MediatR;

namespace Platform.Application.Organization;

public sealed record UpdateDepartmentCommand(Guid TenantId, Guid DepartmentId, UpdateDepartmentRequest Request)
    : IRequest<DepartmentOperationResult>;
