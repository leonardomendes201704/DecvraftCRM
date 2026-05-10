using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class UpdateFinancialAccountCommandHandler
    : IRequestHandler<UpdateFinancialAccountCommand, FinancialAccountOperationResult>
{
    private readonly IFinancialAccountRepository _accountRepository;
    private readonly IClock _clock;

    public UpdateFinancialAccountCommandHandler(IFinancialAccountRepository accountRepository, IClock clock)
    {
        _accountRepository = accountRepository;
        _clock = clock;
    }

    public async Task<FinancialAccountOperationResult> Handle(
        UpdateFinancialAccountCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Name))
        {
            return FinancialAccountOperationResult.InvalidInput();
        }

        var account = await _accountRepository.GetByIdAsync(
            request.TenantId,
            request.AccountId,
            cancellationToken);

        if (account is null)
        {
            return FinancialAccountOperationResult.NotFound();
        }

        var name = request.Request.Name.Trim();
        var exists = await _accountRepository.NameExistsAsync(
            request.TenantId,
            name,
            request.AccountId,
            cancellationToken);

        if (exists)
        {
            return FinancialAccountOperationResult.DuplicateName();
        }

        account.Update(name, _clock.UtcNow);
        await _accountRepository.SaveChangesAsync(cancellationToken);

        return FinancialAccountOperationResult.Success(FinancialAccountResponseMapper.ToResponse(account));
    }
}
