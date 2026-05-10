using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class ListFinancialTransactionsByAccountQueryHandler
    : IRequestHandler<ListFinancialTransactionsByAccountQuery, IReadOnlyCollection<FinancialTransactionResponse>>
{
    private readonly IFinancialTransactionRepository _transactionRepository;

    public ListFinancialTransactionsByAccountQueryHandler(IFinancialTransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<IReadOnlyCollection<FinancialTransactionResponse>> Handle(
        ListFinancialTransactionsByAccountQuery request,
        CancellationToken cancellationToken)
    {
        var account = await _transactionRepository.GetAccountByIdAsync(
            request.TenantId,
            request.AccountId,
            cancellationToken);

        if (account is null)
        {
            return [];
        }

        var transactions = await _transactionRepository.ListByAccountAsync(
            request.TenantId,
            request.AccountId,
            cancellationToken);

        return transactions
            .Select(FinancialTransactionResponseMapper.ToResponse)
            .ToArray();
    }
}
