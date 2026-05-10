using MediatR;

namespace Platform.Application.Finance;

public sealed record GetFinancialAccountByIdQuery(Guid TenantId, Guid AccountId)
    : IRequest<FinancialAccountResponse?>;
