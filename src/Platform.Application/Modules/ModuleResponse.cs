namespace Platform.Application.Modules;

public sealed record ModuleResponse(
    Guid Id,
    string Name,
    string Slug,
    string Version,
    bool IsCore,
    bool IsEnabled,
    bool IsInstalled,
    bool IsActiveForTenant);
