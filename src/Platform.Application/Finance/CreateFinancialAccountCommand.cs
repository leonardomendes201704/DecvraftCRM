using MediatR;

namespace Platform.Application.Finance;

public sealed record CreateFinancialAccountCommand(
    Guid TenantId,
    CreateFinancialAccountRequest Request) : IRequest<FinancialAccountOperationResult>;
