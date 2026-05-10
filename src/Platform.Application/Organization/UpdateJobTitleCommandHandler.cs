using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class UpdateJobTitleCommandHandler
    : IRequestHandler<UpdateJobTitleCommand, JobTitleOperationResult>
{
    private readonly IJobTitleRepository _jobTitleRepository;
    private readonly IClock _clock;

    public UpdateJobTitleCommandHandler(IJobTitleRepository jobTitleRepository, IClock clock)
    {
        _jobTitleRepository = jobTitleRepository;
        _clock = clock;
    }

    public async Task<JobTitleOperationResult> Handle(
        UpdateJobTitleCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Name)
            || string.IsNullOrWhiteSpace(request.Request.Code)
            || request.Request.Level <= 0)
        {
            return JobTitleOperationResult.InvalidInput();
        }

        var jobTitle = await _jobTitleRepository.GetByIdAsync(
            request.TenantId,
            request.JobTitleId,
            cancellationToken);

        if (jobTitle is null)
        {
            return JobTitleOperationResult.NotFound();
        }

        var exists = await _jobTitleRepository.ExistsByCodeAsync(
            request.TenantId,
            request.Request.Code,
            request.JobTitleId,
            cancellationToken);

        if (exists)
        {
            return JobTitleOperationResult.DuplicateCode();
        }

        jobTitle.Update(
            request.Request.Name,
            request.Request.Code,
            request.Request.Level,
            request.Request.IsLeadership,
            _clock.UtcNow);
        await _jobTitleRepository.SaveChangesAsync(cancellationToken);

        return JobTitleOperationResult.Success(JobTitleResponseMapper.ToResponse(jobTitle));
    }
}
