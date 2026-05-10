using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class UpdateFinancialAccountCommandHandler
    : IRequestHandler<UpdateFinancialAccountCommand, FinancialAccountOperationResult>
{
    private readonly IFinancialAccountService _accountService;

    public UpdateFinancialAccountCommandHandler(IFinancialAccountService accountService)
    {
        _accountService = accountService;
    }

    public Task<FinancialAccountOperationResult> Handle(
        UpdateFinancialAccountCommand request,
        CancellationToken cancellationToken)
    {
        return _accountService.UpdateAsync(
            request.TenantId,
            request.AccountId,
            request.Request,
            cancellationToken);
    }
}
