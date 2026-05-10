using Microsoft.AspNetCore.Mvc;
using Platform.Persistence;
using Platform.Persistence.Configuration;
using Platform.Provisioning;
using Platform.Provisioning.Abstractions;
using Platform.Provisioning.Models;
using Platform.Provisioning.Validation;
using Platform.WebInstaller.Filters;
using Platform.WebInstaller.Services;
using Platform.WebInstaller.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<InstalledWizardGuardFilter>();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddFolderApplicationModelConvention(
        "/Install",
        model => model.Filters.Add(new ServiceFilterAttribute(typeof(InstalledWizardGuardFilter))));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".DevcraftCRM.Installer";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddScoped<IInstallerWizardStateStore, SessionInstallerWizardStateStore>();

var defaultConnectionString = builder.Configuration.GetConnectionString(ConnectionStringNames.DefaultConnection)
    ?? builder.Configuration.GetConnectionString(ConnectionStringNames.LegacyDefault);

if (string.IsNullOrWhiteSpace(defaultConnectionString))
{
    throw new InvalidOperationException(
        "Bootstrap connection string 'DefaultConnection' was not configured. Configure it with User Secrets or the process environment before starting the Web Installer.");
}

builder.Services.AddPersistence(defaultConnectionString);
builder.Services.AddProvisioning();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapRazorPages();

app.MapGet(InstallerRoutes.Status, async (IInstallerLockService installerLockService, CancellationToken cancellationToken) =>
{
    try
    {
        var status = await installerLockService.GetStatusAsync(cancellationToken);
        return Results.Ok(status);
    }
    catch
    {
        return Results.Ok(new InstallStatusResult { IsInstalled = false });
    }
})
.WithName(InstallerEndpointNames.Status)
.WithOpenApi();

app.MapPost(InstallerRoutes.TestDatabase, async (
    DatabaseSetupOptions request,
    IDatabaseProvisioner databaseProvisioner,
    CancellationToken cancellationToken) =>
{
    try
    {
        await databaseProvisioner.TestConnectionAsync(request, cancellationToken);
        return Results.Ok(new DatabaseTestResponse(true));
    }
    catch (Exception exception)
    {
        return Results.BadRequest(new DatabaseTestResponse(false, exception.Message));
    }
})
.WithName(InstallerEndpointNames.TestDatabase)
.WithOpenApi();

app.MapPost(InstallerRoutes.Run, async (
    InstallRequest request,
    IProvisioningService provisioningService,
    CancellationToken cancellationToken) =>
{
    var result = await provisioningService.InstallAsync(request, cancellationToken);

    return result.Succeeded
        ? Results.Ok(result)
        : Results.BadRequest(result);
})
.WithName(InstallerEndpointNames.Run)
.WithOpenApi();

app.Run();

internal sealed record DatabaseTestResponse(bool Succeeded, string? Error = null);
