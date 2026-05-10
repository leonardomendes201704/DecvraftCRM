using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class GetFinancialTransactionByIdQueryHandler
    : IRequestHandler<GetFinancialTransactionByIdQuery, FinancialTransactionResponse?>
{
    private readonly IFinancialTransactionService _transactionService;

    public GetFinancialTransactionByIdQueryHandler(IFinancialTransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public Task<FinancialTransactionResponse?> Handle(
        GetFinancialTransactionByIdQuery request,
        CancellationToken cancellationToken)
    {
        return _transactionService.GetByIdAsync(
            request.TenantId,
            request.TransactionId,
            cancellationToken);
    }
}
