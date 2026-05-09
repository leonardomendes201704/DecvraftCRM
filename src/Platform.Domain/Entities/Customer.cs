using Platform.Domain.Common;
using Platform.Domain.Enums;

namespace Platform.Domain.Entities;

public sealed class Customer : ITenantEntity
{
    private Customer()
    {
    }

    private Customer(
        Guid tenantId,
        string name,
        string document,
        CustomerType type,
        DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Name = name;
        Document = document;
        Type = type;
        Status = CustomerStatus.Active;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Document { get; private set; } = string.Empty;
    public CustomerType Type { get; private set; }
    public CustomerStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public static Customer Create(
        Guid tenantId,
        string name,
        string document,
        CustomerType type,
        DateTimeOffset createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(document);

        return new Customer(tenantId, name.Trim(), document.Trim(), type, createdAt);
    }

    public void Update(string name, string document, CustomerType type, DateTimeOffset updatedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(document);

        Name = name.Trim();
        Document = document.Trim();
        Type = type;
        UpdatedAt = updatedAt;
    }

    public void Deactivate(DateTimeOffset updatedAt)
    {
        Status = CustomerStatus.Inactive;
        UpdatedAt = updatedAt;
    }
}
