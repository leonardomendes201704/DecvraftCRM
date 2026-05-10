using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Opportunities;

public sealed class UpdateOpportunityCommandHandler
    : IRequestHandler<UpdateOpportunityCommand, OpportunityOperationResult>
{
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IClock _clock;

    public UpdateOpportunityCommandHandler(IOpportunityRepository opportunityRepository, IClock clock)
    {
        _opportunityRepository = opportunityRepository;
        _clock = clock;
    }

    public async Task<OpportunityOperationResult> Handle(
        UpdateOpportunityCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Title) || request.Request.EstimatedValue < decimal.Zero)
        {
            return OpportunityOperationResult.InvalidInput();
        }

        var opportunity = await _opportunityRepository.GetByIdAsync(
            request.TenantId,
            request.OpportunityId,
            cancellationToken);

        if (opportunity is null)
        {
            return OpportunityOperationResult.NotFound();
        }

        opportunity.Update(
            request.Request.Title,
            request.Request.EstimatedValue,
            request.Request.ExpectedCloseDate,
            _clock.UtcNow);
        await _opportunityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(OpportunityResponseMapper.ToResponse(opportunity));
    }
}
