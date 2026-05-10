using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Opportunities;

public sealed class OpportunityRepository : IOpportunityRepository
{
    private readonly AppDbContext _dbContext;

    public OpportunityRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Opportunity>> ListByCustomerAsync(
        Guid tenantId,
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Opportunities
            .Where(opportunity => opportunity.TenantId == tenantId && opportunity.CustomerId == customerId)
            .OrderByDescending(opportunity => opportunity.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<Opportunity?> GetByIdAsync(Guid tenantId, Guid opportunityId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Opportunities.SingleOrDefaultAsync(
            opportunity => opportunity.TenantId == tenantId && opportunity.Id == opportunityId,
            cancellationToken);
    }

    public async Task<IReadOnlyCollection<Opportunity>> ListByStageAsync(
        Guid tenantId,
        Guid stageId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Opportunities
            .Where(opportunity => opportunity.TenantId == tenantId && opportunity.StageId == stageId)
            .OrderBy(opportunity => opportunity.ExpectedCloseDate)
            .ThenByDescending(opportunity => opportunity.EstimatedValue)
            .ThenBy(opportunity => opportunity.Title)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> CustomerExistsAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Customers
            .AnyAsync(customer => customer.TenantId == tenantId && customer.Id == customerId, cancellationToken);
    }

    public void Add(Opportunity opportunity)
    {
        _dbContext.Opportunities.Add(opportunity);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
