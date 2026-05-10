using Platform.Domain.Enums;

namespace Platform.Application.Finance;

public sealed record FinancialTransactionOperationResult(
    FinancialTransactionOperationStatus Status,
    FinancialTransactionResponse? Transaction)
{
    public static FinancialTransactionOperationResult Success(FinancialTransactionResponse transaction) =>
        new(FinancialTransactionOperationStatus.Success, transaction);

    public static FinancialTransactionOperationResult NotFound() =>
        new(FinancialTransactionOperationStatus.NotFound, null);

    public static FinancialTransactionOperationResult AccountNotFound() =>
        new(FinancialTransactionOperationStatus.AccountNotFound, null);

    public static FinancialTransactionOperationResult InvalidInput() =>
        new(FinancialTransactionOperationStatus.InvalidInput, null);
}
