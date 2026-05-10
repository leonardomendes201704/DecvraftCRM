using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class MarkOpportunityAsWonCommandHandler
    : IRequestHandler<MarkOpportunityAsWonCommand, OpportunityOperationResult>
{
    private readonly IOpportunityService _opportunityService;

    public MarkOpportunityAsWonCommandHandler(IOpportunityService opportunityService)
    {
        _opportunityService = opportunityService;
    }

    public Task<OpportunityOperationResult> Handle(
        MarkOpportunityAsWonCommand request,
        CancellationToken cancellationToken)
    {
        return _opportunityService.MarkAsWonAsync(
            request.TenantId,
            request.OpportunityId,
            cancellationToken);
    }
}
