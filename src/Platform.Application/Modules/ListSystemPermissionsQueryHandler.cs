using MediatR;
using Platform.Domain.Catalog;

namespace Platform.Application.Modules;

public sealed class ListSystemPermissionsQueryHandler
    : IRequestHandler<ListSystemPermissionsQuery, IReadOnlyCollection<PermissionDefinition>>
{
    public Task<IReadOnlyCollection<PermissionDefinition>> Handle(
        ListSystemPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(KnownPermissions.All);
    }
}
