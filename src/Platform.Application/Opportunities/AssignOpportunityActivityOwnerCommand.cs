using MediatR;

namespace Platform.Application.Opportunities;

public sealed record AssignOpportunityActivityOwnerCommand(
    Guid TenantId,
    Guid ActivityId,
    AssignOpportunityActivityOwnerRequest Request) : IRequest<OpportunityActivityOperationResult>;
