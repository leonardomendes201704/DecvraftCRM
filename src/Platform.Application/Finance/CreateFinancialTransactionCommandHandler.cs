using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class CreateFinancialTransactionCommandHandler
    : IRequestHandler<CreateFinancialTransactionCommand, FinancialTransactionOperationResult>
{
    private readonly IFinancialTransactionService _transactionService;

    public CreateFinancialTransactionCommandHandler(IFinancialTransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public Task<FinancialTransactionOperationResult> Handle(
        CreateFinancialTransactionCommand request,
        CancellationToken cancellationToken)
    {
        return _transactionService.CreateAsync(
            request.TenantId,
            request.AccountId,
            request.Request,
            cancellationToken);
    }
}
