namespace Platform.Domain.Catalog;

public static class KnownModules
{
    public const string CoreSlug = "core";
    public const string CrmSlug = "crm";
    public const string FinanceSlug = "finance";

    public static readonly ModuleDefinition Core = new(CoreSlug, "Core", "1.0.0", true);
    public static readonly ModuleDefinition Crm = new(CrmSlug, "CRM", "1.0.0", false);
    public static readonly ModuleDefinition Finance = new(FinanceSlug, "Financeiro", "1.0.0", false);

    public static IReadOnlyCollection<ModuleDefinition> All { get; } =
    [
        Core,
        Crm,
        Finance
    ];

    public static IReadOnlyCollection<string> DefaultSlugs { get; } =
    [
        CoreSlug,
        CrmSlug,
        FinanceSlug
    ];
}
