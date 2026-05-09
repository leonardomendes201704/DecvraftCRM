namespace Platform.Domain.Catalog;

public static class KnownRoles
{
    public const string SystemAdmin = "SystemAdmin";
    public const string TenantAdmin = "TenantAdmin";
    public const string Manager = "Manager";
    public const string User = "User";

    public static IReadOnlyCollection<string> TenantDefaults { get; } =
    [
        TenantAdmin,
        Manager,
        User
    ];
}
