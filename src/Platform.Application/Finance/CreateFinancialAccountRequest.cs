namespace Platform.Application.Finance;

public sealed record CreateFinancialAccountRequest(
    string Name,
    decimal OpeningBalance);
