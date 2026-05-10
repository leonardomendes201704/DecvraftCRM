using Platform.Domain.Entities;

namespace Platform.Application.Abstractions;

public interface IOpportunityStageRepository
{
    Task<IReadOnlyCollection<OpportunityStage>> ListAsync(Guid tenantId, CancellationToken cancellationToken = default);

    Task<OpportunityStage?> GetByIdAsync(Guid tenantId, Guid stageId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(Guid tenantId, string name, Guid? exceptStageId = null, CancellationToken cancellationToken = default);

    void Add(OpportunityStage stage);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
