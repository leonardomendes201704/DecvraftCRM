using MediatR;

namespace Platform.Application.Organization;

public sealed record DeactivateDepartmentCommand(Guid TenantId, Guid DepartmentId)
    : IRequest<DepartmentOperationResult>;
