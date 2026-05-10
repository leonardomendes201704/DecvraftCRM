using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListOpportunitiesByCustomerQuery(Guid TenantId, Guid CustomerId, Guid? OwnerEmployeeId = null)
    : IRequest<IReadOnlyCollection<OpportunityResponse>>;
