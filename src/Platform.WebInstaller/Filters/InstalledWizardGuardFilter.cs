using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Platform.Provisioning.Abstractions;
using Platform.WebInstaller.Wizard;

namespace Platform.WebInstaller.Filters;

public sealed class InstalledWizardGuardFilter : IAsyncPageFilter
{
    private static readonly HashSet<string> AllowedInstalledPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        InstallerWizardRoutes.Welcome,
        InstallerWizardRoutes.Complete
    };

    private readonly IInstallerLockService _installerLockService;

    public InstalledWizardGuardFilter(IInstallerLockService installerLockService)
    {
        _installerLockService = installerLockService;
    }

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        return Task.CompletedTask;
    }

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        var currentPath = context.HttpContext.Request.Path.Value ?? string.Empty;

        if (!AllowedInstalledPaths.Contains(currentPath)
            && await _installerLockService.IsInstalledAsync(context.HttpContext.RequestAborted))
        {
            context.Result = new RedirectResult(InstallerWizardRoutes.Welcome);
            return;
        }

        await next();
    }
}
