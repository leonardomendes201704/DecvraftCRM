using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class GetJobTitleByIdQueryHandler : IRequestHandler<GetJobTitleByIdQuery, JobTitleResponse?>
{
    private readonly IJobTitleRepository _jobTitleRepository;

    public GetJobTitleByIdQueryHandler(IJobTitleRepository jobTitleRepository)
    {
        _jobTitleRepository = jobTitleRepository;
    }

    public async Task<JobTitleResponse?> Handle(GetJobTitleByIdQuery request, CancellationToken cancellationToken)
    {
        var jobTitle = await _jobTitleRepository.GetByIdAsync(
            request.TenantId,
            request.JobTitleId,
            cancellationToken);

        return jobTitle is null ? null : JobTitleResponseMapper.ToResponse(jobTitle);
    }
}
