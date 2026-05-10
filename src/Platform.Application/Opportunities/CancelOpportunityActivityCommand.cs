using MediatR;

namespace Platform.Application.Opportunities;

public sealed record CancelOpportunityActivityCommand(Guid TenantId, Guid ActivityId)
    : IRequest<OpportunityActivityOperationResult>;
