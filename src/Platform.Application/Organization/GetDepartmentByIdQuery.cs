using MediatR;

namespace Platform.Application.Organization;

public sealed record GetDepartmentByIdQuery(Guid TenantId, Guid DepartmentId) : IRequest<DepartmentResponse?>;
