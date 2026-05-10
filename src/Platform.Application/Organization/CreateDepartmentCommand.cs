using MediatR;

namespace Platform.Application.Organization;

public sealed record CreateDepartmentCommand(Guid TenantId, CreateDepartmentRequest Request)
    : IRequest<DepartmentOperationResult>;
