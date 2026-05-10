using MediatR;
using Platform.Domain.Catalog;

namespace Platform.Application.Modules;

public sealed record ListSystemPermissionsQuery : IRequest<IReadOnlyCollection<PermissionDefinition>>;
