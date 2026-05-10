using MediatR;

namespace Platform.Application.Organization;

public sealed record GetJobTitleByIdQuery(Guid TenantId, Guid JobTitleId) : IRequest<JobTitleResponse?>;
