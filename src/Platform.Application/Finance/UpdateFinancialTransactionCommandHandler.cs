using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Enums;

namespace Platform.Application.Finance;

public sealed class UpdateFinancialTransactionCommandHandler
    : IRequestHandler<UpdateFinancialTransactionCommand, FinancialTransactionOperationResult>
{
    private readonly IFinancialTransactionRepository _transactionRepository;
    private readonly IClock _clock;

    public UpdateFinancialTransactionCommandHandler(IFinancialTransactionRepository transactionRepository, IClock clock)
    {
        _transactionRepository = transactionRepository;
        _clock = clock;
    }

    public async Task<FinancialTransactionOperationResult> Handle(
        UpdateFinancialTransactionCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Description) || request.Request.Amount <= decimal.Zero)
        {
            return FinancialTransactionOperationResult.InvalidInput();
        }

        var transaction = await _transactionRepository.GetByIdAsync(
            request.TenantId,
            request.TransactionId,
            cancellationToken);

        if (transaction is null)
        {
            return FinancialTransactionOperationResult.NotFound();
        }

        var account = await _transactionRepository.GetAccountByIdAsync(
            request.TenantId,
            transaction.AccountId,
            cancellationToken);

        if (account is null)
        {
            return FinancialTransactionOperationResult.AccountNotFound();
        }

        if (transaction.Status == FinancialTransactionStatus.Posted)
        {
            account.Reverse(transaction.Type, transaction.Amount, _clock.UtcNow);
            account.Apply(request.Request.Type, request.Request.Amount, _clock.UtcNow);
        }

        transaction.Update(
            request.Request.Description,
            request.Request.Amount,
            request.Request.Type,
            request.Request.OccurredOn,
            _clock.UtcNow);
        await _transactionRepository.SaveChangesAsync(cancellationToken);

        return FinancialTransactionOperationResult.Success(FinancialTransactionResponseMapper.ToResponse(transaction));
    }
}
