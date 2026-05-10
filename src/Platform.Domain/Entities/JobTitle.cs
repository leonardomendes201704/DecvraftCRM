using Platform.Domain.Common;

namespace Platform.Domain.Entities;

public sealed class JobTitle : ITenantEntity
{
    private JobTitle()
    {
    }

    private JobTitle(
        Guid tenantId,
        string name,
        string code,
        int level,
        bool isLeadership,
        DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Name = name;
        Code = code;
        Level = level;
        IsLeadership = isLeadership;
        IsActive = true;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public int Level { get; private set; }
    public bool IsLeadership { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public static JobTitle Create(
        Guid tenantId,
        string name,
        string code,
        int level,
        bool isLeadership,
        DateTimeOffset createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ValidateLevel(level);

        return new JobTitle(tenantId, name.Trim(), code.Trim(), level, isLeadership, createdAt);
    }

    public void Update(string name, string code, int level, bool isLeadership, DateTimeOffset updatedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ValidateLevel(level);

        Name = name.Trim();
        Code = code.Trim();
        Level = level;
        IsLeadership = isLeadership;
        UpdatedAt = updatedAt;
    }

    public void Deactivate(DateTimeOffset updatedAt)
    {
        IsActive = false;
        UpdatedAt = updatedAt;
    }

    private static void ValidateLevel(int level)
    {
        if (level <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }
    }
}
