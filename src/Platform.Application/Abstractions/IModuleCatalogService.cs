using Platform.Application.Modules;

namespace Platform.Application.Abstractions;

public interface IModuleCatalogService
{
    Task<IReadOnlyCollection<ModuleResponse>> GetModulesAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default);
}
