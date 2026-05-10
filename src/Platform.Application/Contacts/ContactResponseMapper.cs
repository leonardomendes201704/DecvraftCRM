using Platform.Domain.Entities;

namespace Platform.Application.Contacts;

internal static class ContactResponseMapper
{
    internal static ContactResponse ToResponse(Contact contact)
    {
        return new ContactResponse(
            contact.Id,
            contact.TenantId,
            contact.CustomerId,
            contact.Name,
            contact.Email,
            contact.Phone,
            contact.Role,
            contact.IsPrimary,
            contact.CreatedAt,
            contact.UpdatedAt);
    }
}
