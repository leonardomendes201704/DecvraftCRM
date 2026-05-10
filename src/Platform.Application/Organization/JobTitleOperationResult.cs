using Platform.Application.Common;
using Platform.Domain.Enums;

namespace Platform.Application.Organization;

public sealed record JobTitleOperationResult(
    OrganizationOperationStatus Status,
    JobTitleResponse? JobTitle) : IApplicationOperationResult<OrganizationOperationStatus>
{
    public bool Succeeded => Status == OrganizationOperationStatus.Success;

    public static JobTitleOperationResult Success(JobTitleResponse jobTitle) =>
        new(OrganizationOperationStatus.Success, jobTitle);

    public static JobTitleOperationResult NotFound() => new(OrganizationOperationStatus.NotFound, null);

    public static JobTitleOperationResult DuplicateCode() => new(OrganizationOperationStatus.DuplicateCode, null);

    public static JobTitleOperationResult InvalidInput() => new(OrganizationOperationStatus.InvalidInput, null);
}
