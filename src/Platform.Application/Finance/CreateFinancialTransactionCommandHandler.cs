using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;

namespace Platform.Application.Finance;

public sealed class CreateFinancialTransactionCommandHandler
    : IRequestHandler<CreateFinancialTransactionCommand, FinancialTransactionOperationResult>
{
    private readonly IFinancialTransactionRepository _transactionRepository;
    private readonly IClock _clock;

    public CreateFinancialTransactionCommandHandler(IFinancialTransactionRepository transactionRepository, IClock clock)
    {
        _transactionRepository = transactionRepository;
        _clock = clock;
    }

    public async Task<FinancialTransactionOperationResult> Handle(
        CreateFinancialTransactionCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Description) || request.Request.Amount <= decimal.Zero)
        {
            return FinancialTransactionOperationResult.InvalidInput();
        }

        var account = await _transactionRepository.GetAccountByIdAsync(
            request.TenantId,
            request.AccountId,
            cancellationToken);

        if (account is null)
        {
            return FinancialTransactionOperationResult.AccountNotFound();
        }

        var transaction = FinancialTransaction.Create(
            request.TenantId,
            request.AccountId,
            request.Request.Description,
            request.Request.Amount,
            request.Request.Type,
            request.Request.OccurredOn,
            _clock.UtcNow);

        account.Apply(transaction.Type, transaction.Amount, _clock.UtcNow);
        _transactionRepository.Add(transaction);
        await _transactionRepository.SaveChangesAsync(cancellationToken);

        return FinancialTransactionOperationResult.Success(FinancialTransactionResponseMapper.ToResponse(transaction));
    }
}
