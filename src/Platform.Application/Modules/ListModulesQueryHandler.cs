using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Modules;

public sealed class ListModulesQueryHandler : IRequestHandler<ListModulesQuery, IReadOnlyCollection<ModuleResponse>>
{
    private readonly IModuleCatalogService _moduleCatalogService;

    public ListModulesQueryHandler(IModuleCatalogService moduleCatalogService)
    {
        _moduleCatalogService = moduleCatalogService;
    }

    public Task<IReadOnlyCollection<ModuleResponse>> Handle(ListModulesQuery request, CancellationToken cancellationToken)
    {
        return _moduleCatalogService.GetModulesAsync(request.TenantId, cancellationToken);
    }
}
