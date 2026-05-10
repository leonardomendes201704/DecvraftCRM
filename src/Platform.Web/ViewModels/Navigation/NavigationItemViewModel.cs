using Platform.Web.Constants;

namespace Platform.Web.ViewModels.Navigation;

public sealed record NavigationItemViewModel(
    string Label,
    string Controller,
    string Action,
    NavigationIconKey IconKey,
    bool IsActive);
