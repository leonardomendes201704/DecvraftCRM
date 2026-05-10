using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListOpportunitiesByStageQuery(Guid TenantId, Guid StageId, Guid? OwnerEmployeeId = null)
    : IRequest<IReadOnlyCollection<OpportunityResponse>>;
