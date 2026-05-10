using Microsoft.EntityFrameworkCore;
using Platform.Application.Opportunities;
using Platform.Domain.Entities;
using Platform.Domain.Enums;
using Platform.Infrastructure.Opportunities;
using Platform.Persistence;

namespace Platform.UnitTests.Opportunities;

public sealed class OpportunityServiceTests
{
    [Fact]
    public async Task CreateAsync_creates_open_opportunity_for_customer_in_same_tenant()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var customer = SeedCustomer(dbContext, tenantId);
        await dbContext.SaveChangesAsync();
        var service = new OpportunityService(dbContext);

        var result = await service.CreateAsync(
            tenantId,
            customer.Id,
            new CreateOpportunityRequest("ERP rollout", 1000, new DateOnly(2026, 6, 30)));

        Assert.Equal(OpportunityOperationStatus.Success, result.Status);
        Assert.NotNull(result.Opportunity);
        Assert.Equal(OpportunityStatus.Open, result.Opportunity.Status);
        Assert.Equal(1000, result.Opportunity.EstimatedValue);
    }

    [Fact]
    public async Task CreateAsync_returns_customer_not_found_for_other_tenant_customer()
    {
        await using var dbContext = CreateDbContext();
        var firstTenantId = Guid.NewGuid();
        var secondTenantId = Guid.NewGuid();
        var customer = SeedCustomer(dbContext, firstTenantId);
        await dbContext.SaveChangesAsync();
        var service = new OpportunityService(dbContext);

        var result = await service.CreateAsync(
            secondTenantId,
            customer.Id,
            new CreateOpportunityRequest("ERP rollout", 1000, null));

        Assert.Equal(OpportunityOperationStatus.CustomerNotFound, result.Status);
    }

    [Fact]
    public async Task CreateAsync_rejects_negative_estimated_value()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var customer = SeedCustomer(dbContext, tenantId);
        await dbContext.SaveChangesAsync();
        var service = new OpportunityService(dbContext);

        var result = await service.CreateAsync(
            tenantId,
            customer.Id,
            new CreateOpportunityRequest("ERP rollout", -1, null));

        Assert.Equal(OpportunityOperationStatus.InvalidInput, result.Status);
    }

    [Fact]
    public async Task ListByCustomerAsync_returns_only_requested_customer_opportunities()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var firstCustomer = SeedCustomer(dbContext, tenantId, "DOC-1");
        var secondCustomer = SeedCustomer(dbContext, tenantId, "DOC-2");
        dbContext.Opportunities.Add(Opportunity.Create(tenantId, firstCustomer.Id, "First", 1000, null, DateTimeOffset.UtcNow));
        dbContext.Opportunities.Add(Opportunity.Create(tenantId, secondCustomer.Id, "Second", 2000, null, DateTimeOffset.UtcNow));
        await dbContext.SaveChangesAsync();
        var service = new OpportunityService(dbContext);

        var opportunities = await service.ListByCustomerAsync(tenantId, firstCustomer.Id);

        var opportunity = Assert.Single(opportunities);
        Assert.Equal("First", opportunity.Title);
    }

    [Fact]
    public async Task UpdateAsync_updates_existing_opportunity()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var customer = SeedCustomer(dbContext, tenantId);
        var opportunity = Opportunity.Create(tenantId, customer.Id, "ERP", 1000, null, DateTimeOffset.UtcNow);
        dbContext.Opportunities.Add(opportunity);
        await dbContext.SaveChangesAsync();
        var service = new OpportunityService(dbContext);

        var result = await service.UpdateAsync(
            tenantId,
            opportunity.Id,
            new UpdateOpportunityRequest("ERP updated", 2000, new DateOnly(2026, 7, 1)));

        Assert.Equal(OpportunityOperationStatus.Success, result.Status);
        Assert.Equal("ERP updated", result.Opportunity!.Title);
        Assert.Equal(2000, result.Opportunity.EstimatedValue);
        Assert.Equal(new DateOnly(2026, 7, 1), result.Opportunity.ExpectedCloseDate);
    }

    [Fact]
    public async Task Status_actions_update_opportunity_status()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var customer = SeedCustomer(dbContext, tenantId);
        var won = Opportunity.Create(tenantId, customer.Id, "Won", 1000, null, DateTimeOffset.UtcNow);
        var lost = Opportunity.Create(tenantId, customer.Id, "Lost", 1000, null, DateTimeOffset.UtcNow);
        var canceled = Opportunity.Create(tenantId, customer.Id, "Canceled", 1000, null, DateTimeOffset.UtcNow);
        dbContext.Opportunities.AddRange(won, lost, canceled);
        await dbContext.SaveChangesAsync();
        var service = new OpportunityService(dbContext);

        var wonResult = await service.MarkAsWonAsync(tenantId, won.Id);
        var lostResult = await service.MarkAsLostAsync(tenantId, lost.Id);
        var canceledResult = await service.CancelAsync(tenantId, canceled.Id);

        Assert.Equal(OpportunityStatus.Won, wonResult.Opportunity!.Status);
        Assert.Equal(OpportunityStatus.Lost, lostResult.Opportunity!.Status);
        Assert.Equal(OpportunityStatus.Canceled, canceledResult.Opportunity!.Status);
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static Customer SeedCustomer(AppDbContext dbContext, Guid tenantId, string document = "DOC-1")
    {
        var customer = Customer.Create(
            tenantId,
            $"Customer {document}",
            document,
            CustomerType.Company,
            DateTimeOffset.UtcNow);
        dbContext.Customers.Add(customer);

        return customer;
    }
}
