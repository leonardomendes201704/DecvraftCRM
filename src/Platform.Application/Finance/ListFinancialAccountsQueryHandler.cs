using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class ListFinancialAccountsQueryHandler
    : IRequestHandler<ListFinancialAccountsQuery, IReadOnlyCollection<FinancialAccountResponse>>
{
    private readonly IFinancialAccountRepository _accountRepository;

    public ListFinancialAccountsQueryHandler(IFinancialAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<IReadOnlyCollection<FinancialAccountResponse>> Handle(
        ListFinancialAccountsQuery request,
        CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.ListAsync(request.TenantId, cancellationToken);

        return accounts
            .Select(FinancialAccountResponseMapper.ToResponse)
            .ToArray();
    }
}
