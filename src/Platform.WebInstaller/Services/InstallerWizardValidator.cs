using Platform.WebInstaller.ViewModels;

namespace Platform.WebInstaller.Services;

public static class InstallerWizardValidator
{
    public static IReadOnlyCollection<string> ValidateDatabase(DatabaseStepViewModel input)
    {
        var errors = new List<string>();

        AddIfEmpty(input.Host, "Informe o host do SQL Server.", errors);
        AddIfEmpty(input.DatabaseName, "Informe o nome do banco.", errors);
        AddIfEmpty(input.Username, "Informe o usuario do banco.", errors);
        AddIfEmpty(input.Password, "Informe a senha do banco.", errors);

        if (input.Port is < 1 or > 65535)
        {
            errors.Add("Informe uma porta valida.");
        }

        return errors;
    }

    public static IReadOnlyCollection<string> ValidateTenant(TenantStepViewModel input)
    {
        var errors = new List<string>();

        AddIfEmpty(input.CompanyName, "Informe o nome da empresa.");
        AddIfEmpty(input.Slug, "Informe o slug do tenant.");

        return errors;

        void AddIfEmpty(string? value, string error)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                errors.Add(error);
            }
        }
    }

    public static IReadOnlyCollection<string> ValidateAdmin(AdminStepViewModel input, string confirmPassword)
    {
        var errors = new List<string>();

        AddIfEmpty(input.Name, "Informe o nome do administrador.", errors);
        AddIfEmpty(input.Email, "Informe o email do administrador.", errors);
        AddIfEmpty(input.Password, "Informe a senha do administrador.", errors);

        if (!string.Equals(input.Password, confirmPassword, StringComparison.Ordinal))
        {
            errors.Add("A confirmacao de senha deve ser igual a senha.");
        }

        return errors;
    }

    public static IReadOnlyCollection<string> ValidateBranding(BrandingStepViewModel input)
    {
        var errors = new List<string>();

        AddIfEmpty(input.SystemName, "Informe o nome comercial.", errors);
        AddIfEmpty(input.PrimaryColor, "Informe a cor primaria.", errors);
        AddIfEmpty(input.SecondaryColor, "Informe a cor secundaria.", errors);

        return errors;
    }

    public static IReadOnlyCollection<string> ValidateReadyToInstall(InstallWizardState state)
    {
        var errors = new List<string>();

        errors.AddRange(ValidateDatabase(state.Database));
        errors.AddRange(ValidateTenant(state.Tenant));
        errors.AddRange(ValidateAdmin(state.Admin, state.Admin.Password));
        errors.AddRange(ValidateBranding(state.Branding));

        if (state.Modules.SelectedModuleSlugs.Count == 0)
        {
            errors.Add("Selecione ao menos o modulo Core.");
        }

        return errors;
    }

    private static void AddIfEmpty(string? value, string error, ICollection<string> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(error);
        }
    }
}
