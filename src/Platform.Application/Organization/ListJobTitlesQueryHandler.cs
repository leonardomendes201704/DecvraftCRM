using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Organization;

public sealed class ListJobTitlesQueryHandler
    : IRequestHandler<ListJobTitlesQuery, IReadOnlyCollection<JobTitleResponse>>
{
    private readonly IJobTitleRepository _jobTitleRepository;

    public ListJobTitlesQueryHandler(IJobTitleRepository jobTitleRepository)
    {
        _jobTitleRepository = jobTitleRepository;
    }

    public async Task<IReadOnlyCollection<JobTitleResponse>> Handle(
        ListJobTitlesQuery request,
        CancellationToken cancellationToken)
    {
        var jobTitles = await _jobTitleRepository.ListAsync(request.TenantId, cancellationToken);

        return jobTitles.Select(JobTitleResponseMapper.ToResponse).ToArray();
    }
}
