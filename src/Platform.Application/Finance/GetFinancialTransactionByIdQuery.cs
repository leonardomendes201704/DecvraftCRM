using MediatR;

namespace Platform.Application.Finance;

public sealed record GetFinancialTransactionByIdQuery(Guid TenantId, Guid TransactionId)
    : IRequest<FinancialTransactionResponse?>;
