using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class GetOpportunityByIdQueryHandler
    : IRequestHandler<GetOpportunityByIdQuery, OpportunityResponse?>
{
    private readonly IOpportunityService _opportunityService;

    public GetOpportunityByIdQueryHandler(IOpportunityService opportunityService)
    {
        _opportunityService = opportunityService;
    }

    public Task<OpportunityResponse?> Handle(
        GetOpportunityByIdQuery request,
        CancellationToken cancellationToken)
    {
        return _opportunityService.GetByIdAsync(
            request.TenantId,
            request.OpportunityId,
            cancellationToken);
    }
}
