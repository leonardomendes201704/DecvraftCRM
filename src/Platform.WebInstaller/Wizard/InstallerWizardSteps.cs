namespace Platform.WebInstaller.Wizard;

public static class InstallerWizardSteps
{
    public static IReadOnlyCollection<InstallerWizardStepDefinition> All { get; } =
    [
        new(InstallerWizardStep.Welcome, "Inicio", InstallerWizardRoutes.Welcome),
        new(InstallerWizardStep.Database, "Banco", InstallerWizardRoutes.Database),
        new(InstallerWizardStep.Tenant, "Tenant", InstallerWizardRoutes.Tenant),
        new(InstallerWizardStep.Admin, "Admin", InstallerWizardRoutes.Admin),
        new(InstallerWizardStep.Branding, "Marca", InstallerWizardRoutes.Branding),
        new(InstallerWizardStep.Modules, "Modulos", InstallerWizardRoutes.Modules),
        new(InstallerWizardStep.Review, "Revisao", InstallerWizardRoutes.Review),
        new(InstallerWizardStep.Complete, "Conclusao", InstallerWizardRoutes.Complete)
    ];

    public static InstallerWizardStepDefinition Get(InstallerWizardStep step)
    {
        return All.Single(definition => definition.Step == step);
    }
}
