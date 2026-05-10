using Platform.Application.Opportunities;

namespace Platform.Application.Abstractions;

public interface IOpportunityService
{
    Task<IReadOnlyCollection<OpportunityResponse>> ListByCustomerAsync(
        Guid tenantId,
        Guid customerId,
        CancellationToken cancellationToken = default);

    Task<OpportunityResponse?> GetByIdAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken = default);

    Task<OpportunityOperationResult> CreateAsync(
        Guid tenantId,
        Guid customerId,
        CreateOpportunityRequest request,
        CancellationToken cancellationToken = default);

    Task<OpportunityOperationResult> UpdateAsync(
        Guid tenantId,
        Guid opportunityId,
        UpdateOpportunityRequest request,
        CancellationToken cancellationToken = default);

    Task<OpportunityOperationResult> MarkAsWonAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken = default);

    Task<OpportunityOperationResult> MarkAsLostAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken = default);

    Task<OpportunityOperationResult> CancelAsync(
        Guid tenantId,
        Guid opportunityId,
        CancellationToken cancellationToken = default);
}
