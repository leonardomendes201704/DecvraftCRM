using MediatR;

namespace Platform.Application.Finance;

public sealed record ListFinancialAccountsQuery(Guid TenantId)
    : IRequest<IReadOnlyCollection<FinancialAccountResponse>>;
