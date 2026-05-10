using Platform.Domain.Entities;

namespace Platform.Application.Customers;

internal static class CustomerResponseMapper
{
    internal static CustomerResponse ToResponse(Customer customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.TenantId,
            customer.Name,
            customer.Document,
            customer.Type,
            customer.Status,
            customer.CreatedAt,
            customer.UpdatedAt);
    }
}
