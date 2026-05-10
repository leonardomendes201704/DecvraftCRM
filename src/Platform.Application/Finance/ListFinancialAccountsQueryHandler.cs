using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class ListFinancialAccountsQueryHandler
    : IRequestHandler<ListFinancialAccountsQuery, IReadOnlyCollection<FinancialAccountResponse>>
{
    private readonly IFinancialAccountService _accountService;

    public ListFinancialAccountsQueryHandler(IFinancialAccountService accountService)
    {
        _accountService = accountService;
    }

    public Task<IReadOnlyCollection<FinancialAccountResponse>> Handle(
        ListFinancialAccountsQuery request,
        CancellationToken cancellationToken)
    {
        return _accountService.ListAsync(request.TenantId, cancellationToken);
    }
}
