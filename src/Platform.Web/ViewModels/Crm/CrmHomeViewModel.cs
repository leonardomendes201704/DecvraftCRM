namespace Platform.Web.ViewModels.Crm;

public sealed record CrmHomeViewModel(
    int OpenOpportunities,
    int ActivitiesDueToday,
    int ActiveAccounts,
    IReadOnlyCollection<string> UpcomingWork);
