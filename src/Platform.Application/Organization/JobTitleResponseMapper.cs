using Platform.Domain.Entities;

namespace Platform.Application.Organization;

internal static class JobTitleResponseMapper
{
    internal static JobTitleResponse ToResponse(JobTitle jobTitle)
    {
        return new JobTitleResponse(
            jobTitle.Id,
            jobTitle.TenantId,
            jobTitle.Name,
            jobTitle.Code,
            jobTitle.Level,
            jobTitle.IsLeadership,
            jobTitle.IsActive,
            jobTitle.CreatedAt,
            jobTitle.UpdatedAt);
    }
}
