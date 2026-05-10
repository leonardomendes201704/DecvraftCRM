using MediatR;

namespace Platform.Application.Organization;

public sealed record DeactivateJobTitleCommand(Guid TenantId, Guid JobTitleId)
    : IRequest<JobTitleOperationResult>;
