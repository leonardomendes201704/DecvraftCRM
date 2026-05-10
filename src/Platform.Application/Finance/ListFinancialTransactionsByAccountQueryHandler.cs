using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class ListFinancialTransactionsByAccountQueryHandler
    : IRequestHandler<ListFinancialTransactionsByAccountQuery, IReadOnlyCollection<FinancialTransactionResponse>>
{
    private readonly IFinancialTransactionService _transactionService;

    public ListFinancialTransactionsByAccountQueryHandler(IFinancialTransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public Task<IReadOnlyCollection<FinancialTransactionResponse>> Handle(
        ListFinancialTransactionsByAccountQuery request,
        CancellationToken cancellationToken)
    {
        return _transactionService.ListByAccountAsync(
            request.TenantId,
            request.AccountId,
            cancellationToken);
    }
}
