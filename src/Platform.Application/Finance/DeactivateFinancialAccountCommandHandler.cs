using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class DeactivateFinancialAccountCommandHandler
    : IRequestHandler<DeactivateFinancialAccountCommand, FinancialAccountOperationResult>
{
    private readonly IFinancialAccountRepository _accountRepository;
    private readonly IClock _clock;

    public DeactivateFinancialAccountCommandHandler(IFinancialAccountRepository accountRepository, IClock clock)
    {
        _accountRepository = accountRepository;
        _clock = clock;
    }

    public async Task<FinancialAccountOperationResult> Handle(
        DeactivateFinancialAccountCommand request,
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(
            request.TenantId,
            request.AccountId,
            cancellationToken);

        if (account is null)
        {
            return FinancialAccountOperationResult.NotFound();
        }

        account.Deactivate(_clock.UtcNow);
        await _accountRepository.SaveChangesAsync(cancellationToken);

        return FinancialAccountOperationResult.Success(FinancialAccountResponseMapper.ToResponse(account));
    }
}
