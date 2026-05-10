using MediatR;

namespace Platform.Application.Finance;

public sealed record VoidFinancialTransactionCommand(Guid TenantId, Guid TransactionId)
    : IRequest<FinancialTransactionOperationResult>;
