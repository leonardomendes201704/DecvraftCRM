using MediatR;

namespace Platform.Application.Finance;

public sealed record ListFinancialTransactionsByAccountQuery(Guid TenantId, Guid AccountId)
    : IRequest<IReadOnlyCollection<FinancialTransactionResponse>>;
