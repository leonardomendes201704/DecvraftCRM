using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class GetFinancialAccountByIdQueryHandler
    : IRequestHandler<GetFinancialAccountByIdQuery, FinancialAccountResponse?>
{
    private readonly IFinancialAccountService _accountService;

    public GetFinancialAccountByIdQueryHandler(IFinancialAccountService accountService)
    {
        _accountService = accountService;
    }

    public Task<FinancialAccountResponse?> Handle(
        GetFinancialAccountByIdQuery request,
        CancellationToken cancellationToken)
    {
        return _accountService.GetByIdAsync(
            request.TenantId,
            request.AccountId,
            cancellationToken);
    }
}
