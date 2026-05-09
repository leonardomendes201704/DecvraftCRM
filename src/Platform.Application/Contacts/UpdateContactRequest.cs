namespace Platform.Application.Contacts;

public sealed record UpdateContactRequest(
    string Name,
    string Email,
    string? Phone,
    string? Role,
    bool IsPrimary);
