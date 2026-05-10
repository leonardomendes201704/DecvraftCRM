using Platform.Domain.Entities;

namespace Platform.Application.Finance;

internal static class FinancialTransactionResponseMapper
{
    internal static FinancialTransactionResponse ToResponse(FinancialTransaction transaction)
    {
        return new FinancialTransactionResponse(
            transaction.Id,
            transaction.TenantId,
            transaction.AccountId,
            transaction.Description,
            transaction.Amount,
            transaction.Type,
            transaction.OccurredOn,
            transaction.Status,
            transaction.CreatedAt,
            transaction.UpdatedAt);
    }
}
