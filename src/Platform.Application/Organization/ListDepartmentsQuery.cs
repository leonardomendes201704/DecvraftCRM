using MediatR;

namespace Platform.Application.Organization;

public sealed record ListDepartmentsQuery(Guid TenantId) : IRequest<IReadOnlyCollection<DepartmentResponse>>;
