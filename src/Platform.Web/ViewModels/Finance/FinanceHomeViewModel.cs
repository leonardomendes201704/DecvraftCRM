namespace Platform.Web.ViewModels.Finance;

public sealed record FinanceHomeViewModel(
    decimal OpenReceivables,
    decimal OpenPayables,
    int PendingDocuments,
    IReadOnlyCollection<string> OperationalChecks);
