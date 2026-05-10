using Platform.Domain.Common;

namespace Platform.Domain.Entities;

public sealed class OpportunityStage : ITenantEntity
{
    private OpportunityStage()
    {
    }

    private OpportunityStage(Guid tenantId, string name, int position, DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Name = name;
        Position = position;
        IsActive = true;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Position { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public static OpportunityStage Create(Guid tenantId, string name, int position, DateTimeOffset createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ValidatePosition(position);

        return new OpportunityStage(tenantId, name.Trim(), position, createdAt);
    }

    public void Update(string name, int position, DateTimeOffset updatedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ValidatePosition(position);

        Name = name.Trim();
        Position = position;
        UpdatedAt = updatedAt;
    }

    public void Deactivate(DateTimeOffset updatedAt)
    {
        IsActive = false;
        UpdatedAt = updatedAt;
    }

    private static void ValidatePosition(int position)
    {
        if (position <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }
    }
}
