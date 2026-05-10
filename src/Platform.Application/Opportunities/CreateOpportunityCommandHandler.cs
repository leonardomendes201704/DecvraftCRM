using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;

namespace Platform.Application.Opportunities;

public sealed class CreateOpportunityCommandHandler
    : IRequestHandler<CreateOpportunityCommand, OpportunityOperationResult>
{
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IClock _clock;

    public CreateOpportunityCommandHandler(IOpportunityRepository opportunityRepository, IClock clock)
    {
        _opportunityRepository = opportunityRepository;
        _clock = clock;
    }

    public async Task<OpportunityOperationResult> Handle(
        CreateOpportunityCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Title) || request.Request.EstimatedValue < decimal.Zero)
        {
            return OpportunityOperationResult.InvalidInput();
        }

        var customerExists = await _opportunityRepository.CustomerExistsAsync(
            request.TenantId,
            request.CustomerId,
            cancellationToken);

        if (!customerExists)
        {
            return OpportunityOperationResult.CustomerNotFound();
        }

        var opportunity = Opportunity.Create(
            request.TenantId,
            request.CustomerId,
            request.Request.Title,
            request.Request.EstimatedValue,
            request.Request.ExpectedCloseDate,
            _clock.UtcNow);

        _opportunityRepository.Add(opportunity);
        await _opportunityRepository.SaveChangesAsync(cancellationToken);

        return OpportunityOperationResult.Success(OpportunityResponseMapper.ToResponse(opportunity));
    }
}
