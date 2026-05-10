using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class CancelOpportunityCommandHandler
    : IRequestHandler<CancelOpportunityCommand, OpportunityOperationResult>
{
    private readonly IOpportunityService _opportunityService;

    public CancelOpportunityCommandHandler(IOpportunityService opportunityService)
    {
        _opportunityService = opportunityService;
    }

    public Task<OpportunityOperationResult> Handle(
        CancelOpportunityCommand request,
        CancellationToken cancellationToken)
    {
        return _opportunityService.CancelAsync(
            request.TenantId,
            request.OpportunityId,
            cancellationToken);
    }
}
