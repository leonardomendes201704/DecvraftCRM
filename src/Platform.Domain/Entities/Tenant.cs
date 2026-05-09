namespace Platform.Domain.Entities;

public sealed class Tenant
{
    private Tenant()
    {
    }

    private Tenant(string name, string slug, string? customDomain, DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        Name = name;
        Slug = slug;
        CustomDomain = customDomain;
        IsActive = true;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? CustomDomain { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static Tenant Create(string name, string slug, string? customDomain, DateTimeOffset createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        return new Tenant(name, slug, customDomain, createdAt);
    }

    public void Deactivate() => IsActive = false;
}
