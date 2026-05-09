using Platform.Domain.Common;

namespace Platform.Domain.Entities;

public sealed class TenantBranding : ITenantEntity
{
    private TenantBranding()
    {
    }

    private TenantBranding(Guid tenantId, string systemName, string primaryColor, string secondaryColor)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        SystemName = systemName;
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string SystemName { get; private set; } = string.Empty;
    public string? LogoUrl { get; private set; }
    public string? FaviconUrl { get; private set; }
    public string PrimaryColor { get; private set; } = "#2563EB";
    public string SecondaryColor { get; private set; } = "#111827";
    public string? LoginBackgroundUrl { get; private set; }
    public string? EmailSenderName { get; private set; }

    public static TenantBranding Create(Guid tenantId, string systemName, string primaryColor, string secondaryColor)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(systemName);
        ArgumentException.ThrowIfNullOrWhiteSpace(primaryColor);
        ArgumentException.ThrowIfNullOrWhiteSpace(secondaryColor);

        return new TenantBranding(tenantId, systemName, primaryColor, secondaryColor);
    }
}
