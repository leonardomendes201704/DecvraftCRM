using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Finance;

public sealed class CreateFinancialAccountCommandHandler
    : IRequestHandler<CreateFinancialAccountCommand, FinancialAccountOperationResult>
{
    private readonly IFinancialAccountService _accountService;

    public CreateFinancialAccountCommandHandler(IFinancialAccountService accountService)
    {
        _accountService = accountService;
    }

    public Task<FinancialAccountOperationResult> Handle(
        CreateFinancialAccountCommand request,
        CancellationToken cancellationToken)
    {
        return _accountService.CreateAsync(request.TenantId, request.Request, cancellationToken);
    }
}
