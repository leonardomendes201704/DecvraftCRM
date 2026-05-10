using Platform.Domain.Entities;

namespace Platform.Application.Abstractions;

public interface IOpportunityRepository
{
    Task<IReadOnlyCollection<Opportunity>> ListByCustomerAsync(
        Guid tenantId,
        Guid customerId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Opportunity>> ListByStageAsync(
        Guid tenantId,
        Guid stageId,
        CancellationToken cancellationToken = default);

    Task<Opportunity?> GetByIdAsync(Guid tenantId, Guid opportunityId, CancellationToken cancellationToken = default);

    Task<bool> CustomerExistsAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default);

    void Add(Opportunity opportunity);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
