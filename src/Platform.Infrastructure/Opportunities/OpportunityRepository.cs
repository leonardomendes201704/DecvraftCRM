using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Application.Opportunities;
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

    public async Task<IReadOnlyCollection<ResponsiblePortfolioSummaryResponse>> ListResponsiblePortfolioSummaryAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        var summaries = await _dbContext.Opportunities
            .Where(opportunity => opportunity.TenantId == tenantId)
            .GroupJoin(
                _dbContext.Employees.Where(employee => employee.TenantId == tenantId),
                opportunity => opportunity.OwnerEmployeeId,
                employee => employee.Id,
                (opportunity, employees) => new { opportunity, employees })
            .SelectMany(
                item => item.employees.DefaultIfEmpty(),
                (item, employee) => new { item.opportunity, employee })
            .GroupBy(item => new
            {
                OwnerEmployeeId = item.employee == null ? null : (Guid?)item.employee.Id,
                OwnerName = item.employee == null ? "Sem responsavel" : item.employee.FullName
            })
            .Select(group => new ResponsiblePortfolioSummaryResponse(
                group.Key.OwnerEmployeeId,
                group.Key.OwnerName,
                group.Count(item => item.opportunity.Status == OpportunityStatus.Open),
                group.Count(item => item.opportunity.Status == OpportunityStatus.Won),
                group.Count(item => item.opportunity.Status == OpportunityStatus.Lost),
                group.Count(item => item.opportunity.Status == OpportunityStatus.Canceled),
                group.Where(item => item.opportunity.Status == OpportunityStatus.Open)
                    .Sum(item => item.opportunity.EstimatedValue)))
            .OrderBy(summary => summary.OwnerName)
            .ToListAsync(cancellationToken);

        return summaries;
    }

    public async Task<IReadOnlyCollection<ResponsibleOpportunitySummaryResponse>> ListResponsibleOpportunitySummaryAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        var summaries = await _dbContext.Opportunities
            .Where(opportunity => opportunity.TenantId == tenantId)
            .GroupJoin(
                _dbContext.Employees.Where(employee => employee.TenantId == tenantId),
                opportunity => opportunity.OwnerEmployeeId,
                employee => employee.Id,
                (opportunity, employees) => new { opportunity, employees })
            .SelectMany(
                item => item.employees.DefaultIfEmpty(),
                (item, employee) => new { item.opportunity, employee })
            .GroupBy(item => new
            {
                OwnerEmployeeId = item.employee == null ? null : (Guid?)item.employee.Id,
                OwnerName = item.employee == null ? "Sem responsavel" : item.employee.FullName
            })
            .Select(group => new ResponsibleOpportunitySummaryResponse(
                group.Key.OwnerEmployeeId,
                group.Key.OwnerName,
                group.Count(),
                group.Count(item => item.opportunity.Status == OpportunityStatus.Open),
                group.Where(item => item.opportunity.Status == OpportunityStatus.Open)
                    .Sum(item => item.opportunity.EstimatedValue)))
            .OrderBy(summary => summary.OwnerName)
            .ToListAsync(cancellationToken);

        return summaries;
    }

    public async Task<IReadOnlyCollection<TeamFunnelSummaryResponse>> ListTeamFunnelSummaryAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        var summaries = await _dbContext.Opportunities
            .Where(opportunity => opportunity.TenantId == tenantId)
            .GroupJoin(
                _dbContext.Employees.Where(employee => employee.TenantId == tenantId),
                opportunity => opportunity.OwnerEmployeeId,
                employee => employee.Id,
                (opportunity, employees) => new { opportunity, employees })
            .SelectMany(
                item => item.employees.DefaultIfEmpty(),
                (item, employee) => new { item.opportunity, employee })
            .GroupJoin(
                _dbContext.Departments.Where(department => department.TenantId == tenantId),
                item => item.employee == null ? null : item.employee.DepartmentId,
                department => department.Id,
                (item, departments) => new { item.opportunity, departments })
            .SelectMany(
                item => item.departments.DefaultIfEmpty(),
                (item, department) => new { item.opportunity, department })
            .GroupBy(item => new
            {
                DepartmentId = item.department == null ? null : (Guid?)item.department.Id,
                DepartmentName = item.department == null ? "Sem departamento" : item.department.Name
            })
            .Select(group => new TeamFunnelSummaryResponse(
                group.Key.DepartmentId,
                group.Key.DepartmentName,
                group.Count(item => item.opportunity.Status == OpportunityStatus.Open),
                group.Count(item => item.opportunity.Status == OpportunityStatus.Won),
                group.Count(item => item.opportunity.Status == OpportunityStatus.Lost),
                group.Count(item => item.opportunity.Status == OpportunityStatus.Canceled),
                group.Where(item => item.opportunity.Status == OpportunityStatus.Open)
                    .Sum(item => item.opportunity.EstimatedValue)))
            .OrderBy(summary => summary.DepartmentName)
            .ToListAsync(cancellationToken);

        return summaries;
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
