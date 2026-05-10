using MediatR;

namespace Platform.Application.Modules;

public sealed record ListModulesQuery(Guid TenantId) : IRequest<IReadOnlyCollection<ModuleResponse>>;
