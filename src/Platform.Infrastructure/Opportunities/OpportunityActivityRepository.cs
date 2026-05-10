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
        Guid? ownerEmployeeId = null,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.OpportunityActivities
            .Where(activity =>
                activity.TenantId == tenantId
                && activity.OpportunityId == opportunityId
                && (ownerEmployeeId == null || activity.OwnerEmployeeId == ownerEmployeeId))
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
        Guid? ownerEmployeeId = null,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.OpportunityActivities
            .Where(activity =>
                activity.TenantId == tenantId
                && activity.Status == OpportunityActivityStatus.Scheduled
                && (ownerEmployeeId == null || activity.OwnerEmployeeId == ownerEmployeeId)
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
        Guid? ownerEmployeeId = null,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.OpportunityActivities
            .Where(activity =>
                activity.TenantId == tenantId
                && activity.Status == OpportunityActivityStatus.Scheduled
                && (ownerEmployeeId == null || activity.OwnerEmployeeId == ownerEmployeeId)
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

    public Task<bool> ActiveEmployeeExistsAsync(
        Guid tenantId,
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Employees.AnyAsync(
            employee =>
                employee.TenantId == tenantId
                && employee.Id == employeeId
                && employee.Status == EmployeeStatus.Active,
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
