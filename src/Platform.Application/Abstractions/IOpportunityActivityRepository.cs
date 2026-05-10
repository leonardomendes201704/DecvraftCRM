using Platform.Application.Opportunities;
using Platform.Domain.Entities;

namespace Platform.Application.Abstractions;

public interface IOpportunityActivityRepository
{
    Task<IReadOnlyCollection<OpportunityActivity>> ListByOpportunityAsync(
        Guid tenantId,
        Guid opportunityId,
        Guid? ownerEmployeeId = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<OpportunityActivity>> ListOverdueAsync(
        Guid tenantId,
        DateTimeOffset now,
        Guid? ownerEmployeeId = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<OpportunityActivity>> ListUpcomingAsync(
        Guid tenantId,
        DateTimeOffset from,
        DateTimeOffset to,
        Guid? ownerEmployeeId = null,
        CancellationToken cancellationToken = default);

    Task<OpportunityActivity?> GetByIdAsync(Guid tenantId, Guid activityId, CancellationToken cancellationToken = default);

    Task<bool> OpportunityExistsAsync(Guid tenantId, Guid opportunityId, CancellationToken cancellationToken = default);

    Task<bool> ActiveEmployeeExistsAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ResponsibleActivitySummaryResponse>> ListOverdueSummaryByResponsibleAsync(
        Guid tenantId,
        DateTimeOffset now,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ResponsibleActivitySummaryResponse>> ListUpcomingSummaryByResponsibleAsync(
        Guid tenantId,
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken cancellationToken = default);

    void Add(OpportunityActivity activity);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
