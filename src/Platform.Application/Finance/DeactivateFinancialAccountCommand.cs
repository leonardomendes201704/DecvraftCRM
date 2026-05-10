using MediatR;

namespace Platform.Application.Finance;

public sealed record DeactivateFinancialAccountCommand(Guid TenantId, Guid AccountId)
    : IRequest<FinancialAccountOperationResult>;
