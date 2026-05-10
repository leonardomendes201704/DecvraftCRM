using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class UpdateOpportunityCommandHandler
    : IRequestHandler<UpdateOpportunityCommand, OpportunityOperationResult>
{
    private readonly IOpportunityService _opportunityService;

    public UpdateOpportunityCommandHandler(IOpportunityService opportunityService)
    {
        _opportunityService = opportunityService;
    }

    public Task<OpportunityOperationResult> Handle(
        UpdateOpportunityCommand request,
        CancellationToken cancellationToken)
    {
        return _opportunityService.UpdateAsync(
            request.TenantId,
            request.OpportunityId,
            request.Request,
            cancellationToken);
    }
}
