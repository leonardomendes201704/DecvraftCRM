using MediatR;

namespace Platform.Application.Organization;

public sealed record ListEmployeeSubordinatesQuery(Guid TenantId, Guid EmployeeId)
    : IRequest<IReadOnlyCollection<EmployeeResponse>>;
