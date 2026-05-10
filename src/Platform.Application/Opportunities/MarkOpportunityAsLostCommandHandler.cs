using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class MarkOpportunityAsLostCommandHandler
    : IRequestHandler<MarkOpportunityAsLostCommand, OpportunityOperationResult>
{
    private readonly IOpportunityService _opportunityService;

    public MarkOpportunityAsLostCommandHandler(IOpportunityService opportunityService)
    {
        _opportunityService = opportunityService;
    }

    public Task<OpportunityOperationResult> Handle(
        MarkOpportunityAsLostCommand request,
        CancellationToken cancellationToken)
    {
        return _opportunityService.MarkAsLostAsync(
            request.TenantId,
            request.OpportunityId,
            cancellationToken);
    }
}
