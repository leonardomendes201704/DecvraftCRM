using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListResponsibleOverdueActivitySummaryQuery(Guid TenantId)
    : IRequest<IReadOnlyCollection<ResponsibleActivitySummaryResponse>>;
