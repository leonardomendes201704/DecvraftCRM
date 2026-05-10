using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Enums;

namespace Platform.Application.Finance;

public sealed class VoidFinancialTransactionCommandHandler
    : IRequestHandler<VoidFinancialTransactionCommand, FinancialTransactionOperationResult>
{
    private readonly IFinancialTransactionRepository _transactionRepository;
    private readonly IClock _clock;

    public VoidFinancialTransactionCommandHandler(IFinancialTransactionRepository transactionRepository, IClock clock)
    {
        _transactionRepository = transactionRepository;
        _clock = clock;
    }

    public async Task<FinancialTransactionOperationResult> Handle(
        VoidFinancialTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(
            request.TenantId,
            request.TransactionId,
            cancellationToken);

        if (transaction is null)
        {
            return FinancialTransactionOperationResult.NotFound();
        }

        if (transaction.Status == FinancialTransactionStatus.Posted)
        {
            var account = await _transactionRepository.GetAccountByIdAsync(
                request.TenantId,
                transaction.AccountId,
                cancellationToken);

            if (account is null)
            {
                return FinancialTransactionOperationResult.AccountNotFound();
            }

            account.Reverse(transaction.Type, transaction.Amount, _clock.UtcNow);
            transaction.Void(_clock.UtcNow);
            await _transactionRepository.SaveChangesAsync(cancellationToken);
        }

        return FinancialTransactionOperationResult.Success(FinancialTransactionResponseMapper.ToResponse(transaction));
    }
}
