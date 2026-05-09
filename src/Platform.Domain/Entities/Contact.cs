using Platform.Domain.Common;

namespace Platform.Domain.Entities;

public sealed class Contact : ITenantEntity
{
    private Contact()
    {
    }

    private Contact(
        Guid tenantId,
        Guid customerId,
        string name,
        string email,
        string? phone,
        string? role,
        DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        CustomerId = customerId;
        Name = name;
        Email = email;
        Phone = phone;
        Role = role;
        IsPrimary = false;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public string? Role { get; private set; }
    public bool IsPrimary { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public static Contact Create(
        Guid tenantId,
        Guid customerId,
        string name,
        string email,
        string? phone,
        string? role,
        DateTimeOffset createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return new Contact(
            tenantId,
            customerId,
            name.Trim(),
            email.Trim().ToLowerInvariant(),
            string.IsNullOrWhiteSpace(phone) ? null : phone.Trim(),
            string.IsNullOrWhiteSpace(role) ? null : role.Trim(),
            createdAt);
    }

    public void MarkAsPrimary(DateTimeOffset updatedAt)
    {
        IsPrimary = true;
        UpdatedAt = updatedAt;
    }

    public void Update(
        string name,
        string email,
        string? phone,
        string? role,
        bool isPrimary,
        DateTimeOffset updatedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
        Role = string.IsNullOrWhiteSpace(role) ? null : role.Trim();
        IsPrimary = isPrimary;
        UpdatedAt = updatedAt;
    }
}
