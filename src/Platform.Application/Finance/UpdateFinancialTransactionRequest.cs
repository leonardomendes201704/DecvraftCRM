using Platform.Domain.Enums;

namespace Platform.Application.Finance;

public sealed record UpdateFinancialTransactionRequest(
    string Description,
    decimal Amount,
    FinancialTransactionType Type,
    DateOnly OccurredOn);
