namespace Platform.Domain.Catalog;

public static class KnownPermissions
{
    public static IReadOnlyCollection<PermissionDefinition> All { get; } =
    [
        new("core.system.view", "Visualizar informacoes do sistema", KnownModules.CoreSlug),
        new("core.system.manage", "Gerenciar configuracoes do sistema", KnownModules.CoreSlug),
        new("core.users.view", "Visualizar usuarios", KnownModules.CoreSlug),
        new("core.users.manage", "Gerenciar usuarios", KnownModules.CoreSlug),
        new("core.modules.view", "Visualizar modulos", KnownModules.CoreSlug),
        new("core.modules.manage", "Gerenciar modulos", KnownModules.CoreSlug),

        new("crm.customers.view", "Visualizar clientes", KnownModules.CrmSlug),
        new("crm.customers.create", "Criar clientes", KnownModules.CrmSlug),
        new("crm.customers.update", "Atualizar clientes", KnownModules.CrmSlug),
        new("crm.customers.delete", "Excluir clientes", KnownModules.CrmSlug),
        new("crm.opportunities.view", "Visualizar oportunidades", KnownModules.CrmSlug),
        new("crm.opportunities.manage", "Gerenciar oportunidades", KnownModules.CrmSlug),

        new("finance.accounts.view", "Visualizar contas financeiras", KnownModules.FinanceSlug),
        new("finance.accounts.manage", "Gerenciar contas financeiras", KnownModules.FinanceSlug),
        new("finance.transactions.view", "Visualizar lancamentos financeiros", KnownModules.FinanceSlug),
        new("finance.transactions.manage", "Gerenciar lancamentos financeiros", KnownModules.FinanceSlug)
    ];
}
