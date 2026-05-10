using MediatR;

namespace Platform.Application.Organization;

public sealed record ListEmployeesQuery(Guid TenantId) : IRequest<IReadOnlyCollection<EmployeeResponse>>;
