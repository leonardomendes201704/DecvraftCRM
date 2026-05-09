namespace Platform.Application.Contacts;

public sealed record CreateContactRequest(
    string Name,
    string Email,
    string? Phone,
    string? Role,
    bool IsPrimary);
