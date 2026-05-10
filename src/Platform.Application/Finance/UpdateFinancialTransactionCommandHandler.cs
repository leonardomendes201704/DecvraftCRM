using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class UpdateFinancialTransactionCommandHandler
    : IRequestHandler<UpdateFinancialTransactionCommand, FinancialTransactionOperationResult>
{
    private readonly IFinancialTransactionService _transactionService;

    public UpdateFinancialTransactionCommandHandler(IFinancialTransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public Task<FinancialTransactionOperationResult> Handle(
        UpdateFinancialTransactionCommand request,
        CancellationToken cancellationToken)
    {
        return _transactionService.UpdateAsync(
            request.TenantId,
            request.TransactionId,
            request.Request,
            cancellationToken);
    }
}
