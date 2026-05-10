namespace Platform.Web.ViewModels.Organization;

public sealed record OrganizationHomeViewModel(
    int Departments,
    int JobTitles,
    int Employees,
    IReadOnlyCollection<string> GovernanceItems);
