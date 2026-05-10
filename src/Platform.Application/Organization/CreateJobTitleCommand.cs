using MediatR;

namespace Platform.Application.Organization;

public sealed record CreateJobTitleCommand(Guid TenantId, CreateJobTitleRequest Request)
    : IRequest<JobTitleOperationResult>;
