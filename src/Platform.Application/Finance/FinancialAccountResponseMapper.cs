using Platform.Domain.Entities;

namespace Platform.Application.Finance;

internal static class FinancialAccountResponseMapper
{
    internal static FinancialAccountResponse ToResponse(FinancialAccount account)
    {
        return new FinancialAccountResponse(
            account.Id,
            account.TenantId,
            account.Name,
            account.OpeningBalance,
            account.CurrentBalance,
            account.Status,
            account.CreatedAt,
            account.UpdatedAt);
    }
}
