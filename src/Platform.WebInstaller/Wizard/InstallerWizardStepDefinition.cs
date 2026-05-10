namespace Platform.WebInstaller.Wizard;

public sealed record InstallerWizardStepDefinition(
    InstallerWizardStep Step,
    string Title,
    string Route);
