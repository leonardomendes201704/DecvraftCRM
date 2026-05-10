namespace Platform.Web.ViewModels.Components;

public sealed record DataTableViewModel(
    IReadOnlyCollection<string> Headers,
    IReadOnlyCollection<IReadOnlyCollection<string>> Rows,
    string EmptyMessage);
