using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListOpportunitiesByStageQuery(Guid TenantId, Guid StageId)
    : IRequest<IReadOnlyCollection<OpportunityResponse>>;
