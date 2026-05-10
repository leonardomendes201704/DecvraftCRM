using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListResponsibleUpcomingActivitySummaryQuery(Guid TenantId, int Days)
    : IRequest<IReadOnlyCollection<ResponsibleActivitySummaryResponse>>;
