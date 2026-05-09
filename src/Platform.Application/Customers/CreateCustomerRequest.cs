using Platform.Domain.Enums;

namespace Platform.Application.Customers;

public sealed record CreateCustomerRequest(
    string Name,
    string Document,
    CustomerType Type);
