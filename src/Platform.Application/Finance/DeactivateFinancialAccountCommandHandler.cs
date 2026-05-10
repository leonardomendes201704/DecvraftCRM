using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class DeactivateFinancialAccountCommandHandler
    : IRequestHandler<DeactivateFinancialAccountCommand, FinancialAccountOperationResult>
{
    private readonly IFinancialAccountService _accountService;

    public DeactivateFinancialAccountCommandHandler(IFinancialAccountService accountService)
    {
        _accountService = accountService;
    }

    public Task<FinancialAccountOperationResult> Handle(
        DeactivateFinancialAccountCommand request,
        CancellationToken cancellationToken)
    {
        return _accountService.DeactivateAsync(
            request.TenantId,
            request.AccountId,
            cancellationToken);
    }
}
