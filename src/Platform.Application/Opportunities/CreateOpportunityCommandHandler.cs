using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class CreateOpportunityCommandHandler
    : IRequestHandler<CreateOpportunityCommand, OpportunityOperationResult>
{
    private readonly IOpportunityService _opportunityService;

    public CreateOpportunityCommandHandler(IOpportunityService opportunityService)
    {
        _opportunityService = opportunityService;
    }

    public Task<OpportunityOperationResult> Handle(
        CreateOpportunityCommand request,
        CancellationToken cancellationToken)
    {
        return _opportunityService.CreateAsync(
            request.TenantId,
            request.CustomerId,
            request.Request,
            cancellationToken);
    }
}
