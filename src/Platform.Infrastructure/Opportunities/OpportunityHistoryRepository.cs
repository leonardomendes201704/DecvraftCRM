using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Opportunities;

public sealed class OpportunityHistoryRepository : IOpportunityHistoryRepository
{
    private readonly AppDbContext _dbContext;

    public OpportunityHistoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<OpportunityHistoryEntry>> ListByOpportunityAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.OpportunityHistoryEntries
            .Where(entry => entry.TenantId == tenantId && entry.OpportunityId == opportunityId)
            .OrderByDescending(entry => entry.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public void Add(OpportunityHistoryEntry entry)
    {
        _dbContext.OpportunityHistoryEntries.Add(entry);
    }
}
