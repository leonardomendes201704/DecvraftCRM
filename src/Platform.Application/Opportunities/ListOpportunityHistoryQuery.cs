using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListOpportunityHistoryQuery(Guid TenantId, Guid OpportunityId)
    : IRequest<IReadOnlyCollection<OpportunityHistoryEntryResponse>>;
