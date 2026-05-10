using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListResponsibleOpportunitySummaryQuery(Guid TenantId)
    : IRequest<IReadOnlyCollection<ResponsibleOpportunitySummaryResponse>>;
