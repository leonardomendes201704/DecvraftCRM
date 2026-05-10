using Platform.Domain.Catalog;

namespace Platform.WebInstaller.ViewModels;

public sealed class ModulesStepViewModel
{
    public bool CoreEnabled { get; set; } = true;
    public bool CrmEnabled { get; set; } = true;
    public bool FinanceEnabled { get; set; } = true;

    public IReadOnlyCollection<string> SelectedModuleSlugs
    {
        get
        {
            var slugs = new List<string> { KnownModules.CoreSlug };

            if (CrmEnabled)
            {
                slugs.Add(KnownModules.CrmSlug);
            }

            if (FinanceEnabled)
            {
                slugs.Add(KnownModules.FinanceSlug);
            }

            return slugs;
        }
    }
}
