using MediatR;

namespace Platform.Application.Organization;

public sealed record ListJobTitlesQuery(Guid TenantId) : IRequest<IReadOnlyCollection<JobTitleResponse>>;
