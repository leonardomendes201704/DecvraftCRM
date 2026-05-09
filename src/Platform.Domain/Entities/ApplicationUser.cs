using Platform.Domain.Common;

namespace Platform.Domain.Entities;

public sealed class ApplicationUser : ITenantEntity
{
    private ApplicationUser()
    {
    }

    private ApplicationUser(Guid tenantId, string name, string email, string passwordHash, DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = true;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static ApplicationUser Create(Guid tenantId, string name, string email, string passwordHash, DateTimeOffset createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        return new ApplicationUser(tenantId, name, email, passwordHash, createdAt);
    }
}
