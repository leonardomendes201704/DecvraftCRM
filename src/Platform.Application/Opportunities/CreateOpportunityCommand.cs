using MediatR;

namespace Platform.Application.Opportunities;

public sealed record CreateOpportunityCommand(
    Guid TenantId,
    Guid CustomerId,
    CreateOpportunityRequest Request) : IRequest<OpportunityOperationResult>;
