using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListTeamFunnelSummaryQuery(Guid TenantId)
    : IRequest<IReadOnlyCollection<TeamFunnelSummaryResponse>>;
