using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.UnitTests.Crm;

public sealed class CrmDomainTests
{
    [Fact]
    public void Customer_Create_sets_default_status_and_trimmed_values()
    {
        var tenantId = Guid.NewGuid();

        var customer = Customer.Create(
            tenantId,
            "  Acme  ",
            "  12345678000190  ",
            CustomerType.Company,
            DateTimeOffset.UtcNow);

        Assert.Equal(tenantId, customer.TenantId);
        Assert.Equal("Acme", customer.Name);
        Assert.Equal("12345678000190", customer.Document);
        Assert.Equal(CustomerStatus.Active, customer.Status);
        Assert.Equal(CustomerType.Company, customer.Type);
    }

    [Fact]
    public void Contact_Create_normalizes_email_and_optional_values()
    {
        var contact = Contact.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            " Maria ",
            " MARIA@ACME.TEST ",
            " ",
            null,
            DateTimeOffset.UtcNow);

        Assert.Equal("Maria", contact.Name);
        Assert.Equal("maria@acme.test", contact.Email);
        Assert.Null(contact.Phone);
        Assert.Null(contact.Role);
        Assert.False(contact.IsPrimary);
    }

    [Fact]
    public void Opportunity_Create_rejects_negative_estimated_value()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Opportunity.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "ERP rollout",
                -1,
                null,
                DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Crm_enums_have_stable_values()
    {
        Assert.Equal(1, (int)CustomerType.Company);
        Assert.Equal(2, (int)CustomerType.Individual);
        Assert.Equal(1, (int)CustomerStatus.Active);
        Assert.Equal(2, (int)CustomerStatus.Inactive);
        Assert.Equal(1, (int)OpportunityStatus.Open);
        Assert.Equal(2, (int)OpportunityStatus.Won);
        Assert.Equal(3, (int)OpportunityStatus.Lost);
        Assert.Equal(4, (int)OpportunityStatus.Canceled);
    }
}
