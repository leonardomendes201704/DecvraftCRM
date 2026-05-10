using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class GetOpportunityByIdQueryHandler
    : IRequestHandler<GetOpportunityByIdQuery, OpportunityResponse?>
{
    private readonly IOpportunityRepository _opportunityRepository;

    public GetOpportunityByIdQueryHandler(IOpportunityRepository opportunityRepository)
    {
        _opportunityRepository = opportunityRepository;
    }

    public async Task<OpportunityResponse?> Handle(
        GetOpportunityByIdQuery request,
        CancellationToken cancellationToken)
    {
        var opportunity = await _opportunityRepository.GetByIdAsync(
            request.TenantId,
            request.OpportunityId,
            cancellationToken);

        return opportunity is null ? null : OpportunityResponseMapper.ToResponse(opportunity);
    }
}
