using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;

namespace Platform.Application.Organization;

public sealed class CreateJobTitleCommandHandler
    : IRequestHandler<CreateJobTitleCommand, JobTitleOperationResult>
{
    private readonly IJobTitleRepository _jobTitleRepository;
    private readonly IClock _clock;

    public CreateJobTitleCommandHandler(IJobTitleRepository jobTitleRepository, IClock clock)
    {
        _jobTitleRepository = jobTitleRepository;
        _clock = clock;
    }

    public async Task<JobTitleOperationResult> Handle(
        CreateJobTitleCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Name)
            || string.IsNullOrWhiteSpace(request.Request.Code)
            || request.Request.Level <= 0)
        {
            return JobTitleOperationResult.InvalidInput();
        }

        var exists = await _jobTitleRepository.ExistsByCodeAsync(
            request.TenantId,
            request.Request.Code,
            cancellationToken: cancellationToken);

        if (exists)
        {
            return JobTitleOperationResult.DuplicateCode();
        }

        var jobTitle = JobTitle.Create(
            request.TenantId,
            request.Request.Name,
            request.Request.Code,
            request.Request.Level,
            request.Request.IsLeadership,
            _clock.UtcNow);

        _jobTitleRepository.Add(jobTitle);
        await _jobTitleRepository.SaveChangesAsync(cancellationToken);

        return JobTitleOperationResult.Success(JobTitleResponseMapper.ToResponse(jobTitle));
    }
}
