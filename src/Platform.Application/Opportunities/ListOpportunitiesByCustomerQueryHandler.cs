using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class ListOpportunitiesByCustomerQueryHandler
    : IRequestHandler<ListOpportunitiesByCustomerQuery, IReadOnlyCollection<OpportunityResponse>>
{
    private readonly IOpportunityRepository _opportunityRepository;

    public ListOpportunitiesByCustomerQueryHandler(IOpportunityRepository opportunityRepository)
    {
        _opportunityRepository = opportunityRepository;
    }

    public async Task<IReadOnlyCollection<OpportunityResponse>> Handle(
        ListOpportunitiesByCustomerQuery request,
        CancellationToken cancellationToken)
    {
        var customerExists = await _opportunityRepository.CustomerExistsAsync(
            request.TenantId,
            request.CustomerId,
            cancellationToken);

        if (!customerExists)
        {
            return [];
        }

        var opportunities = await _opportunityRepository.ListByCustomerAsync(
            request.TenantId,
            request.CustomerId,
            request.OwnerEmployeeId,
            cancellationToken);

        return opportunities
            .Select(OpportunityResponseMapper.ToResponse)
            .ToArray();
    }
}
