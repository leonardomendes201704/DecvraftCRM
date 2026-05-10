using Platform.Domain.Enums;

namespace Platform.Application.Finance;

public sealed record FinancialAccountResponse(
    Guid Id,
    Guid TenantId,
    string Name,
    decimal OpeningBalance,
    decimal CurrentBalance,
    FinancialAccountStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
