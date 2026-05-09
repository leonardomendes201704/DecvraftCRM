using Platform.Domain.Enums;

namespace Platform.Application.Customers;

public sealed record UpdateCustomerRequest(
    string Name,
    string Document,
    CustomerType Type);
