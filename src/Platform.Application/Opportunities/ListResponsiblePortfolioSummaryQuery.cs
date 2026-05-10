using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListResponsiblePortfolioSummaryQuery(Guid TenantId)
    : IRequest<IReadOnlyCollection<ResponsiblePortfolioSummaryResponse>>;
