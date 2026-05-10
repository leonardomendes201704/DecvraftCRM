using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Application.Opportunities;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Opportunities;

public sealed class OpportunityService : IOpportunityService
{
    private readonly AppDbContext _dbContext;

    public OpportunityService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<OpportunityResponse>> ListByCustomerAsync(
        Guid tenantId,
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        var customerExists = await _dbContext.Customers
            .AnyAsync(customer => customer.TenantId == tenantId && customer.Id == customerId, cancellationToken);

        if (!customerExists)
        {
            return [];
        }

        return await _dbContext.Opportunities
            .Where(opportunity => opportunity.TenantId == tenantId && opportunity.CustomerId == customerId)
            .OrderBy(opportunity => opportunity.ExpectedCloseDate)
            .ThenBy(opportunity => opportunity.Title)
            .Select(opportunity => new OpportunityResponse(
                opportunity.Id,
                opportunity.TenantId,
                opportunity.CustomerId,
                opportunity.StageId,
                opportunity.OwnerEmployeeId,
                opportunity.Title,
                opportunity.EstimatedValue,
                opportunity.ExpectedCloseDate,
                opportunity.Status,
                opportunity.CreatedAt,
                opportunity.UpdatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<OpportunityResponse?> GetByIdAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken = default)
    {
        var opportunity = await _dbContext.Opportunities
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == opportunityId, cancellationToken);

        return opportunity is null ? null : ToResponse(opportunity);
    }

    public async Task<OpportunityOperationResult> CreateAsync(
        Guid tenantId,
        Guid customerId,
        CreateOpportunityRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || request.EstimatedValue < decimal.Zero)
        {
            return OpportunityOperationResult.InvalidInput();
        }

        var customerExists = await _dbContext.Customers
            .AnyAsync(customer => customer.TenantId == tenantId && customer.Id == customerId, cancellationToken);

        if (!customerExists)
        {
            return OpportunityOperationResult.CustomerNotFound();
        }

        var opportunity = Opportunity.Create(
            tenantId,
            customerId,
            request.Title,
            request.EstimatedValue,
            request.ExpectedCloseDate,
            DateTimeOffset.UtcNow,
            request.StageId);

        _dbContext.Opportunities.Add(opportunity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(ToResponse(opportunity));
    }

    public async Task<OpportunityOperationResult> UpdateAsync(
        Guid tenantId,
        Guid opportunityId,
        UpdateOpportunityRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || request.EstimatedValue < decimal.Zero)
        {
            return OpportunityOperationResult.InvalidInput();
        }

        var opportunity = await _dbContext.Opportunities
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == opportunityId, cancellationToken);

        if (opportunity is null)
        {
            return OpportunityOperationResult.NotFound();
        }

        opportunity.Update(
            request.Title,
            request.EstimatedValue,
            request.ExpectedCloseDate,
            DateTimeOffset.UtcNow,
            request.StageId);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(ToResponse(opportunity));
    }

    public async Task<OpportunityOperationResult> MarkAsWonAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken = default)
    {
        var opportunity = await FindOpportunityAsync(tenantId, opportunityId, cancellationToken);
        if (opportunity is null)
        {
            return OpportunityOperationResult.NotFound();
        }

        opportunity.MarkAsWon(DateTimeOffset.UtcNow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(ToResponse(opportunity));
    }

    public async Task<OpportunityOperationResult> MarkAsLostAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken = default)
    {
        var opportunity = await FindOpportunityAsync(tenantId, opportunityId, cancellationToken);
        if (opportunity is null)
        {
            return OpportunityOperationResult.NotFound();
        }

        opportunity.MarkAsLost(DateTimeOffset.UtcNow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(ToResponse(opportunity));
    }

    public async Task<OpportunityOperationResult> CancelAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken = default)
    {
        var opportunity = await FindOpportunityAsync(tenantId, opportunityId, cancellationToken);
        if (opportunity is null)
        {
            return OpportunityOperationResult.NotFound();
        }

        opportunity.Cancel(DateTimeOffset.UtcNow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(ToResponse(opportunity));
    }

    private async Task<Opportunity?> FindOpportunityAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Opportunities
            .SingleOrDefaultAsync(item => item.TenantId == tenantId && item.Id == opportunityId, cancellationToken);
    }

    private static OpportunityResponse ToResponse(Opportunity opportunity)
    {
        return new OpportunityResponse(
            opportunity.Id,
            opportunity.TenantId,
            opportunity.CustomerId,
            opportunity.StageId,
            opportunity.OwnerEmployeeId,
            opportunity.Title,
            opportunity.EstimatedValue,
            opportunity.ExpectedCloseDate,
            opportunity.Status,
            opportunity.CreatedAt,
            opportunity.UpdatedAt);
    }
}
