using Platform.Domain.Enums;

namespace Platform.Application.Finance;

public sealed record FinancialAccountOperationResult(
    FinancialAccountOperationStatus Status,
    FinancialAccountResponse? Account)
{
    public static FinancialAccountOperationResult Success(FinancialAccountResponse account) =>
        new(FinancialAccountOperationStatus.Success, account);

    public static FinancialAccountOperationResult NotFound() =>
        new(FinancialAccountOperationStatus.NotFound, null);

    public static FinancialAccountOperationResult DuplicateName() =>
        new(FinancialAccountOperationStatus.DuplicateName, null);

    public static FinancialAccountOperationResult InvalidInput() =>
        new(FinancialAccountOperationStatus.InvalidInput, null);
}
