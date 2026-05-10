using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class CancelOpportunityCommandHandler
    : IRequestHandler<CancelOpportunityCommand, OpportunityOperationResult>
{
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IClock _clock;

    public CancelOpportunityCommandHandler(IOpportunityRepository opportunityRepository, IClock clock)
    {
        _opportunityRepository = opportunityRepository;
        _clock = clock;
    }

    public async Task<OpportunityOperationResult> Handle(
        CancelOpportunityCommand request,
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

        opportunity.Cancel(_clock.UtcNow);
        await _opportunityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(OpportunityResponseMapper.ToResponse(opportunity));
    }
}
