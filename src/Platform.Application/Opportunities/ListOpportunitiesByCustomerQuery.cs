using MediatR;

namespace Platform.Application.Opportunities;

public sealed record ListOpportunitiesByCustomerQuery(Guid TenantId, Guid CustomerId)
    : IRequest<IReadOnlyCollection<OpportunityResponse>>;
