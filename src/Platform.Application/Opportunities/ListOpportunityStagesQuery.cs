using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListOpportunityStagesQuery(Guid TenantId) : IRequest<IReadOnlyCollection<OpportunityStageResponse>>;
