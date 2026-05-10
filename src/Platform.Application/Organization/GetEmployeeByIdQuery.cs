using MediatR;

namespace Platform.Application.Organization;

public sealed record GetEmployeeByIdQuery(Guid TenantId, Guid EmployeeId) : IRequest<EmployeeResponse?>;
