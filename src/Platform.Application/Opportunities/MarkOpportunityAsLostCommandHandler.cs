using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class MarkOpportunityAsLostCommandHandler
    : IRequestHandler<MarkOpportunityAsLostCommand, OpportunityOperationResult>
{
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IClock _clock;

    public MarkOpportunityAsLostCommandHandler(IOpportunityRepository opportunityRepository, IClock clock)
    {
        _opportunityRepository = opportunityRepository;
        _clock = clock;
    }

    public async Task<OpportunityOperationResult> Handle(
        MarkOpportunityAsLostCommand request,
        CancellationToken cancellationToken)
    {
        var opportunity = await _opportunityRepository.GetByIdAsync(
            request.TenantId,
            request.OpportunityId,
            cancellationToken);

        if (opportunity is null)
        {
            return OpportunityOperationResult.NotFound();
        }

        opportunity.MarkAsLost(_clock.UtcNow);
        await _opportunityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(OpportunityResponseMapper.ToResponse(opportunity));
    }
}
