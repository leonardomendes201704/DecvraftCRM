using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class VoidFinancialTransactionCommandHandler
    : IRequestHandler<VoidFinancialTransactionCommand, FinancialTransactionOperationResult>
{
    private readonly IFinancialTransactionService _transactionService;

    public VoidFinancialTransactionCommandHandler(IFinancialTransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public Task<FinancialTransactionOperationResult> Handle(
        VoidFinancialTransactionCommand request,
        CancellationToken cancellationToken)
    {
        return _transactionService.VoidAsync(
            request.TenantId,
            request.TransactionId,
            cancellationToken);
    }
}
