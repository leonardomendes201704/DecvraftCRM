using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class DeactivateJobTitleCommandHandler
    : IRequestHandler<DeactivateJobTitleCommand, JobTitleOperationResult>
{
    private readonly IJobTitleRepository _jobTitleRepository;
    private readonly IClock _clock;

    public DeactivateJobTitleCommandHandler(IJobTitleRepository jobTitleRepository, IClock clock)
    {
        _jobTitleRepository = jobTitleRepository;
        _clock = clock;
    }

    public async Task<JobTitleOperationResult> Handle(
        DeactivateJobTitleCommand request,
        CancellationToken cancellationToken)
    {
        var jobTitle = await _jobTitleRepository.GetByIdAsync(
            request.TenantId,
            request.JobTitleId,
            cancellationToken);

        if (jobTitle is null)
        {
            return JobTitleOperationResult.NotFound();
        }

        jobTitle.Deactivate(_clock.UtcNow);
        await _jobTitleRepository.SaveChangesAsync(cancellationToken);

        return JobTitleOperationResult.Success(JobTitleResponseMapper.ToResponse(jobTitle));
    }
}
