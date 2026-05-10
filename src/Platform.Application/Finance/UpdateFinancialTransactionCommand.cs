using MediatR;

namespace Platform.Application.Finance;

public sealed record UpdateFinancialTransactionCommand(
    Guid TenantId,
    Guid TransactionId,
    UpdateFinancialTransactionRequest Request) : IRequest<FinancialTransactionOperationResult>;
