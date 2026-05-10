using MediatR;

namespace Platform.Application.Opportunities;

public sealed record UpdateOpportunityActivityCommand(
    Guid TenantId,
    Guid ActivityId,
    UpdateOpportunityActivityRequest Request) : IRequest<OpportunityActivityOperationResult>;
