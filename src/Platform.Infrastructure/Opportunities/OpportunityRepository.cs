using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Domain.Enums;
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
        Guid? ownerEmployeeId = null,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Opportunities
            .Where(opportunity =>
                opportunity.TenantId == tenantId
                && opportunity.CustomerId == customerId
                && (ownerEmployeeId == null || opportunity.OwnerEmployeeId == ownerEmployeeId))
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
        Guid? ownerEmployeeId = null,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Opportunities
            .Where(opportunity =>
                opportunity.TenantId == tenantId
                && opportunity.StageId == stageId
                && (ownerEmployeeId == null || opportunity.OwnerEmployeeId == ownerEmployeeId))
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

    public void Add(Opportunity opportunity)
    {
        _dbContext.Opportunities.Add(opportunity);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
