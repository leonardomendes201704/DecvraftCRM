using Platform.Web.ViewModels.Components;

namespace Platform.Web.ViewModels.Dashboard;

public sealed record DashboardViewModel(
    IReadOnlyCollection<DashboardKpiViewModel> Kpis,
    DataTableViewModel WorkQueue,
    IReadOnlyCollection<string> Priorities);
