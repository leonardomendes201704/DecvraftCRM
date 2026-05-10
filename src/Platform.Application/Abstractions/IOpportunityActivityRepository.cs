using Platform.Domain.Entities;

namespace Platform.Application.Abstractions;

public interface IOpportunityActivityRepository
{
    Task<IReadOnlyCollection<OpportunityActivity>> ListByOpportunityAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<OpportunityActivity>> ListOverdueAsync(
        Guid tenantId,
        DateTimeOffset now,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<OpportunityActivity>> ListUpcomingAsync(
        Guid tenantId,
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken cancellationToken = default);

    Task<OpportunityActivity?> GetByIdAsync(Guid tenantId, Guid activityId, CancellationToken cancellationToken = default);

    Task<bool> OpportunityExistsAsync(Guid tenantId, Guid opportunityId, CancellationToken cancellationToken = default);

    void Add(OpportunityActivity activity);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
