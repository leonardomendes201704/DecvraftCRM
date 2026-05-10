using Platform.Domain.Entities;

namespace Platform.Application.Abstractions;

public interface IOpportunityHistoryRepository
{
    Task<IReadOnlyCollection<OpportunityHistoryEntry>> ListByOpportunityAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken = default);

    void Add(OpportunityHistoryEntry entry);
}
