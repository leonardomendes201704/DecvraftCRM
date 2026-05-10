using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class GetFinancialAccountByIdQueryHandler
    : IRequestHandler<GetFinancialAccountByIdQuery, FinancialAccountResponse?>
{
    private readonly IFinancialAccountRepository _accountRepository;

    public GetFinancialAccountByIdQueryHandler(IFinancialAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<FinancialAccountResponse?> Handle(
        GetFinancialAccountByIdQuery request,
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(
            request.TenantId,
            request.AccountId,
            cancellationToken);

        return account is null ? null : FinancialAccountResponseMapper.ToResponse(account);
    }
}
