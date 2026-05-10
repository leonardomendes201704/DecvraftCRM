using Platform.Application.Opportunities;
using Platform.Domain.Entities;

namespace Platform.Application.Abstractions;

public interface IOpportunityRepository
{
    Task<IReadOnlyCollection<Opportunity>> ListByCustomerAsync(
        Guid tenantId,
        Guid customerId,
        Guid? ownerEmployeeId = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Opportunity>> ListByStageAsync(
        Guid tenantId,
        Guid stageId,
        Guid? ownerEmployeeId = null,
        CancellationToken cancellationToken = default);

    Task<Opportunity?> GetByIdAsync(Guid tenantId, Guid opportunityId, CancellationToken cancellationToken = default);

    Task<bool> CustomerExistsAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default);

    Task<bool> ActiveEmployeeExistsAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ResponsiblePortfolioSummaryResponse>> ListResponsiblePortfolioSummaryAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ResponsibleOpportunitySummaryResponse>> ListResponsibleOpportunitySummaryAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TeamFunnelSummaryResponse>> ListTeamFunnelSummaryAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default);

    void Add(Opportunity opportunity);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
