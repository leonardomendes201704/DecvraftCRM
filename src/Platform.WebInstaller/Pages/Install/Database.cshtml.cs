using Microsoft.AspNetCore.Mvc;
using Platform.Provisioning.Abstractions;
using Platform.WebInstaller.Services;
using Platform.WebInstaller.ViewModels;
using Platform.WebInstaller.Wizard;

namespace Platform.WebInstaller.Pages.Install;

public sealed class DatabaseModel : InstallPageModel
{
    private readonly IDatabaseProvisioner _databaseProvisioner;

    public DatabaseModel(
        IInstallerWizardStateStore stateStore,
        IDatabaseProvisioner databaseProvisioner)
        : base(stateStore)
    {
        _databaseProvisioner = databaseProvisioner;
    }

    public override InstallerWizardStep CurrentStep => InstallerWizardStep.Database;

    [BindProperty]
    public DatabaseStepViewModel Input { get; set; } = new();

    public void OnGet()
    {
        Input = LoadWizardState().Database;
    }

    public async Task<IActionResult> OnPostAsync(string command, CancellationToken cancellationToken)
    {
        var state = LoadWizardState();
        PageErrors = InstallerWizardValidator.ValidateDatabase(Input);

        if (PageErrors.Count > 0)
        {
            return Page();
        }

        if (string.Equals(command, "test", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                await _databaseProvisioner.TestConnectionAsync(
                    InstallerWizardRequestFactory.ToDatabaseOptions(Input),
                    cancellationToken);

                Input.HasConnectionTest = true;
                Input.LastConnectionTestSucceeded = true;
                PageSuccessMessage = "Conexao testada com sucesso.";
            }
            catch (Exception exception)
            {
                Input.HasConnectionTest = true;
                Input.LastConnectionTestSucceeded = false;
                PageErrors = [$"Nao foi possivel conectar ao banco: {exception.Message}"];
            }

            state.Database = Input;
            SaveWizardState(state);

            return Page();
        }

        if (!Input.LastConnectionTestSucceeded)
        {
            PageErrors = ["Teste a conexao com sucesso antes de continuar."];
            return Page();
        }

        state.Database = Input;
        SaveWizardState(state);

        return Redirect(InstallerWizardRoutes.Tenant);
    }
}
