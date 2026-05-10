using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class GetFinancialTransactionByIdQueryHandler
    : IRequestHandler<GetFinancialTransactionByIdQuery, FinancialTransactionResponse?>
{
    private readonly IFinancialTransactionRepository _transactionRepository;

    public GetFinancialTransactionByIdQueryHandler(IFinancialTransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<FinancialTransactionResponse?> Handle(
        GetFinancialTransactionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(
            request.TenantId,
            request.TransactionId,
            cancellationToken);

        return transaction is null ? null : FinancialTransactionResponseMapper.ToResponse(transaction);
    }
}
