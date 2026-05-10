using MediatR;

namespace Platform.Application.Finance;

public sealed record CreateFinancialTransactionCommand(
    Guid TenantId,
    Guid AccountId,
    CreateFinancialTransactionRequest Request) : IRequest<FinancialTransactionOperationResult>;
