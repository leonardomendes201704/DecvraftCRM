namespace Platform.Provisioning.Models;

public sealed class ProvisioningResult
{
    private ProvisioningResult(bool succeeded, Guid? tenantId, Guid? adminUserId, IReadOnlyCollection<string> installedModules, IReadOnlyCollection<string> errors)
    {
        Succeeded = succeeded;
        TenantId = tenantId;
        AdminUserId = adminUserId;
        InstalledModules = installedModules;
        Errors = errors;
    }

    public bool Succeeded { get; }
    public Guid? TenantId { get; }
    public Guid? AdminUserId { get; }
    public IReadOnlyCollection<string> InstalledModules { get; }
    public IReadOnlyCollection<string> Errors { get; }

    public static ProvisioningResult Success(Guid tenantId, Guid adminUserId, IReadOnlyCollection<string> installedModules)
    {
        return new ProvisioningResult(true, tenantId, adminUserId, installedModules, []);
    }

    public static ProvisioningResult Failure(IReadOnlyCollection<string> errors)
    {
        return new ProvisioningResult(false, null, null, [], errors);
    }
}
