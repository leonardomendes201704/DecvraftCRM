using MediatR;

namespace Platform.Application.Opportunities;

public sealed record AssignOpportunityOwnerCommand(
    Guid TenantId,
    Guid OpportunityId,
    AssignOpportunityOwnerRequest Request) : IRequest<OpportunityOperationResult>;
