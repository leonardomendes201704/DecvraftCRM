using Platform.Domain.Enums;

namespace Platform.Application.Finance;

public sealed record FinancialTransactionResponse(
    Guid Id,
    Guid TenantId,
    Guid AccountId,
    string Description,
    decimal Amount,
    FinancialTransactionType Type,
    DateOnly OccurredOn,
    FinancialTransactionStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
