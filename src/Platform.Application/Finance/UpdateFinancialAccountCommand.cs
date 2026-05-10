using MediatR;

namespace Platform.Application.Finance;

public sealed record UpdateFinancialAccountCommand(
    Guid TenantId,
    Guid AccountId,
    UpdateFinancialAccountRequest Request) : IRequest<FinancialAccountOperationResult>;
