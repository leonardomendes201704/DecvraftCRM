using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;
using Platform.Persistence;

namespace Platform.Infrastructure.Opportunities;

public sealed class OpportunityActivityRepository : IOpportunityActivityRepository
{
    private readonly AppDbContext _dbContext;

    public OpportunityActivityRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<OpportunityActivity>> ListByOpportunityAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.OpportunityActivities
            .Where(activity => activity.TenantId == tenantId && activity.OpportunityId == opportunityId)
            .OrderBy(activity => activity.Status)
            .ThenBy(activity => activity.DueAt)
            .ThenByDescending(activity => activity.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<OpportunityActivity?> GetByIdAsync(
        Guid tenantId,
        Guid activityId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.OpportunityActivities.SingleOrDefaultAsync(
            activity => activity.TenantId == tenantId && activity.Id == activityId,
            cancellationToken);
    }

    public async Task<IReadOnlyCollection<OpportunityActivity>> ListOverdueAsync(
        Guid tenantId,
        DateTimeOffset now,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.OpportunityActivities
            .Where(activity =>
                activity.TenantId == tenantId
                && activity.Status == OpportunityActivityStatus.Scheduled
                && activity.DueAt != null
                && activity.DueAt < now)
            .OrderBy(activity => activity.DueAt)
            .ThenBy(activity => activity.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<OpportunityActivity>> ListUpcomingAsync(
        Guid tenantId,
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.OpportunityActivities
            .Where(activity =>
                activity.TenantId == tenantId
                && activity.Status == OpportunityActivityStatus.Scheduled
                && activity.DueAt != null
                && activity.DueAt >= from
                && activity.DueAt <= to)
            .OrderBy(activity => activity.DueAt)
            .ThenBy(activity => activity.Title)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> OpportunityExistsAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Opportunities.AnyAsync(
            opportunity => opportunity.TenantId == tenantId && opportunity.Id == opportunityId,
            cancellationToken);
    }

    public void Add(OpportunityActivity activity)
    {
        _dbContext.OpportunityActivities.Add(activity);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
