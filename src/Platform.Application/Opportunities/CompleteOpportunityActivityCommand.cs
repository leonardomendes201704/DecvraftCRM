using MediatR;

namespace Platform.Application.Opportunities;

public sealed record CompleteOpportunityActivityCommand(Guid TenantId, Guid ActivityId)
    : IRequest<OpportunityActivityOperationResult>;
