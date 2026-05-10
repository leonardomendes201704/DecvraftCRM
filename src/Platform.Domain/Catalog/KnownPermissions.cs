namespace Platform.Domain.Catalog;

public static class KnownPermissions
{
    public const string CoreSystemView = "core.system.view";
    public const string CoreSystemManage = "core.system.manage";
    public const string CoreUsersView = "core.users.view";
    public const string CoreUsersManage = "core.users.manage";
    public const string CoreModulesView = "core.modules.view";
    public const string CoreModulesManage = "core.modules.manage";
    public const string CoreDepartmentsView = "core.departments.view";
    public const string CoreDepartmentsManage = "core.departments.manage";
    public const string CoreJobTitlesView = "core.job-titles.view";
    public const string CoreJobTitlesManage = "core.job-titles.manage";
    public const string CoreEmployeesView = "core.employees.view";
    public const string CoreEmployeesManage = "core.employees.manage";
    public const string CoreHierarchyView = "core.hierarchy.view";
    public const string CoreHierarchyManage = "core.hierarchy.manage";
    public const string CrmCustomersView = "crm.customers.view";
    public const string CrmCustomersCreate = "crm.customers.create";
    public const string CrmCustomersUpdate = "crm.customers.update";
    public const string CrmCustomersDelete = "crm.customers.delete";
    public const string CrmOpportunitiesView = "crm.opportunities.view";
    public const string CrmOpportunitiesManage = "crm.opportunities.manage";
    public const string FinanceAccountsView = "finance.accounts.view";
    public const string FinanceAccountsManage = "finance.accounts.manage";
    public const string FinanceTransactionsView = "finance.transactions.view";
    public const string FinanceTransactionsManage = "finance.transactions.manage";

    public static IReadOnlyCollection<PermissionDefinition> All { get; } =
    [
        new(CoreSystemView, "Visualizar informacoes do sistema", KnownModules.CoreSlug),
        new(CoreSystemManage, "Gerenciar configuracoes do sistema", KnownModules.CoreSlug),
        new(CoreUsersView, "Visualizar usuarios", KnownModules.CoreSlug),
        new(CoreUsersManage, "Gerenciar usuarios", KnownModules.CoreSlug),
        new(CoreModulesView, "Visualizar modulos", KnownModules.CoreSlug),
        new(CoreModulesManage, "Gerenciar modulos", KnownModules.CoreSlug),
        new(CoreDepartmentsView, "Visualizar departamentos", KnownModules.CoreSlug),
        new(CoreDepartmentsManage, "Gerenciar departamentos", KnownModules.CoreSlug),
        new(CoreJobTitlesView, "Visualizar cargos", KnownModules.CoreSlug),
        new(CoreJobTitlesManage, "Gerenciar cargos", KnownModules.CoreSlug),
        new(CoreEmployeesView, "Visualizar funcionarios", KnownModules.CoreSlug),
        new(CoreEmployeesManage, "Gerenciar funcionarios", KnownModules.CoreSlug),
        new(CoreHierarchyView, "Visualizar hierarquia", KnownModules.CoreSlug),
        new(CoreHierarchyManage, "Gerenciar hierarquia", KnownModules.CoreSlug),

        new(CrmCustomersView, "Visualizar clientes", KnownModules.CrmSlug),
        new(CrmCustomersCreate, "Criar clientes", KnownModules.CrmSlug),
        new(CrmCustomersUpdate, "Atualizar clientes", KnownModules.CrmSlug),
        new(CrmCustomersDelete, "Excluir clientes", KnownModules.CrmSlug),
        new(CrmOpportunitiesView, "Visualizar oportunidades", KnownModules.CrmSlug),
        new(CrmOpportunitiesManage, "Gerenciar oportunidades", KnownModules.CrmSlug),

        new(FinanceAccountsView, "Visualizar contas financeiras", KnownModules.FinanceSlug),
        new(FinanceAccountsManage, "Gerenciar contas financeiras", KnownModules.FinanceSlug),
        new(FinanceTransactionsView, "Visualizar lancamentos financeiros", KnownModules.FinanceSlug),
        new(FinanceTransactionsManage, "Gerenciar lancamentos financeiros", KnownModules.FinanceSlug)
    ];
}
