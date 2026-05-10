using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class ListOpportunitiesByCustomerQueryHandler
    : IRequestHandler<ListOpportunitiesByCustomerQuery, IReadOnlyCollection<OpportunityResponse>>
{
    private readonly IOpportunityService _opportunityService;

    public ListOpportunitiesByCustomerQueryHandler(IOpportunityService opportunityService)
    {
        _opportunityService = opportunityService;
    }

    public Task<IReadOnlyCollection<OpportunityResponse>> Handle(
        ListOpportunitiesByCustomerQuery request,
        CancellationToken cancellationToken)
    {
        return _opportunityService.ListByCustomerAsync(
            request.TenantId,
            request.CustomerId,
            cancellationToken);
    }
}
