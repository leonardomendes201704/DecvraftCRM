using Microsoft.EntityFrameworkCore;
using Platform.Application.Contacts;
using Platform.Domain.Entities;
using Platform.Domain.Enums;
using Platform.Infrastructure.Contacts;
using Platform.Persistence;

namespace Platform.UnitTests.Contacts;

public sealed class ContactServiceTests
{
    [Fact]
    public async Task CreateAsync_creates_contact_for_customer_in_same_tenant()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var customer = SeedCustomer(dbContext, tenantId);
        await dbContext.SaveChangesAsync();
        var service = new ContactService(dbContext);

        var result = await service.CreateAsync(
            tenantId,
            customer.Id,
            new CreateContactRequest("Maria", "MARIA@ACME.TEST", null, "Buyer", true));

        Assert.Equal(ContactOperationStatus.Success, result.Status);
        Assert.NotNull(result.Contact);
        Assert.Equal("maria@acme.test", result.Contact.Email);
        Assert.True(result.Contact.IsPrimary);
    }

    [Fact]
    public async Task CreateAsync_returns_customer_not_found_for_other_tenant_customer()
    {
        await using var dbContext = CreateDbContext();
        var firstTenantId = Guid.NewGuid();
        var secondTenantId = Guid.NewGuid();
        var customer = SeedCustomer(dbContext, firstTenantId);
        await dbContext.SaveChangesAsync();
        var service = new ContactService(dbContext);

        var result = await service.CreateAsync(
            secondTenantId,
            customer.Id,
            new CreateContactRequest("Maria", "maria@acme.test", null, null, false));

        Assert.Equal(ContactOperationStatus.CustomerNotFound, result.Status);
    }

    [Fact]
    public async Task CreateAsync_rejects_duplicate_email_inside_same_customer()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var customer = SeedCustomer(dbContext, tenantId);
        dbContext.Contacts.Add(Contact.Create(
            tenantId,
            customer.Id,
            "Maria",
            "maria@acme.test",
            null,
            null,
            DateTimeOffset.UtcNow));
        await dbContext.SaveChangesAsync();
        var service = new ContactService(dbContext);

        var result = await service.CreateAsync(
            tenantId,
            customer.Id,
            new CreateContactRequest("Maria 2", "MARIA@ACME.TEST", null, null, false));

        Assert.Equal(ContactOperationStatus.DuplicateEmail, result.Status);
    }

    [Fact]
    public async Task ListByCustomerAsync_returns_only_requested_customer_contacts()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var firstCustomer = SeedCustomer(dbContext, tenantId, "DOC-1");
        var secondCustomer = SeedCustomer(dbContext, tenantId, "DOC-2");
        dbContext.Contacts.Add(Contact.Create(tenantId, firstCustomer.Id, "Maria", "maria@acme.test", null, null, DateTimeOffset.UtcNow));
        dbContext.Contacts.Add(Contact.Create(tenantId, secondCustomer.Id, "Joao", "joao@acme.test", null, null, DateTimeOffset.UtcNow));
        await dbContext.SaveChangesAsync();
        var service = new ContactService(dbContext);

        var contacts = await service.ListByCustomerAsync(tenantId, firstCustomer.Id);

        var contact = Assert.Single(contacts);
        Assert.Equal("Maria", contact.Name);
    }

    [Fact]
    public async Task UpdateAsync_updates_existing_contact()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var customer = SeedCustomer(dbContext, tenantId);
        var contact = Contact.Create(tenantId, customer.Id, "Maria", "maria@acme.test", null, null, DateTimeOffset.UtcNow);
        dbContext.Contacts.Add(contact);
        await dbContext.SaveChangesAsync();
        var service = new ContactService(dbContext);

        var result = await service.UpdateAsync(
            tenantId,
            contact.Id,
            new UpdateContactRequest("Maria Updated", "updated@acme.test", "11999990000", "Director", true));

        Assert.Equal(ContactOperationStatus.Success, result.Status);
        Assert.Equal("Maria Updated", result.Contact!.Name);
        Assert.Equal("updated@acme.test", result.Contact.Email);
        Assert.Equal("11999990000", result.Contact.Phone);
        Assert.Equal("Director", result.Contact.Role);
        Assert.True(result.Contact.IsPrimary);
    }

    [Fact]
    public async Task DeleteAsync_removes_contact()
    {
        await using var dbContext = CreateDbContext();
        var tenantId = Guid.NewGuid();
        var customer = SeedCustomer(dbContext, tenantId);
        var contact = Contact.Create(tenantId, customer.Id, "Maria", "maria@acme.test", null, null, DateTimeOffset.UtcNow);
        dbContext.Contacts.Add(contact);
        await dbContext.SaveChangesAsync();
        var service = new ContactService(dbContext);

        var result = await service.DeleteAsync(tenantId, contact.Id);

        Assert.Equal(ContactOperationStatus.Success, result.Status);
        Assert.False(await dbContext.Contacts.AnyAsync());
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
