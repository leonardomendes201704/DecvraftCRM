using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;

namespace Platform.Application.Finance;

public sealed class CreateFinancialAccountCommandHandler
    : IRequestHandler<CreateFinancialAccountCommand, FinancialAccountOperationResult>
{
    private readonly IFinancialAccountRepository _accountRepository;
    private readonly IClock _clock;

    public CreateFinancialAccountCommandHandler(IFinancialAccountRepository accountRepository, IClock clock)
    {
        _accountRepository = accountRepository;
        _clock = clock;
    }

    public async Task<FinancialAccountOperationResult> Handle(
        CreateFinancialAccountCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Name))
        {
            return FinancialAccountOperationResult.InvalidInput();
        }

        var name = request.Request.Name.Trim();
        var exists = await _accountRepository.NameExistsAsync(
            request.TenantId,
            name,
            null,
            cancellationToken);

        if (exists)
        {
            return FinancialAccountOperationResult.DuplicateName();
        }

        var account = FinancialAccount.Create(request.TenantId, name, request.Request.OpeningBalance, _clock.UtcNow);
        _accountRepository.Add(account);
        await _accountRepository.SaveChangesAsync(cancellationToken);

        return FinancialAccountOperationResult.Success(FinancialAccountResponseMapper.ToResponse(account));
    }
}
