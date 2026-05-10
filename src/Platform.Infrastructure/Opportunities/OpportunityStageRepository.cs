using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Opportunities;

public sealed class OpportunityStageRepository : IOpportunityStageRepository
{
    private readonly AppDbContext _dbContext;

    public OpportunityStageRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<OpportunityStage>> ListAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.OpportunityStages
            .Where(stage => stage.TenantId == tenantId)
            .OrderBy(stage => stage.Position)
            .ThenBy(stage => stage.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<OpportunityStage?> GetByIdAsync(
        Guid tenantId,
        Guid stageId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.OpportunityStages.SingleOrDefaultAsync(
            stage => stage.TenantId == tenantId && stage.Id == stageId,
            cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(
        Guid tenantId,
        string name,
        Guid? exceptStageId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim();

        return _dbContext.OpportunityStages.AnyAsync(
            stage =>
                stage.TenantId == tenantId
                && stage.Name == normalizedName
                && (exceptStageId == null || stage.Id != exceptStageId),
            cancellationToken);
    }

    public void Add(OpportunityStage stage)
    {
        _dbContext.OpportunityStages.Add(stage);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
