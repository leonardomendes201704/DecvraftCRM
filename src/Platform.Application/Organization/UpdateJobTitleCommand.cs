using MediatR;

namespace Platform.Application.Organization;

public sealed record UpdateJobTitleCommand(Guid TenantId, Guid JobTitleId, UpdateJobTitleRequest Request)
    : IRequest<JobTitleOperationResult>;
