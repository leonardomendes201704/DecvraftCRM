using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Contacts;

public sealed class UpdateContactCommandHandler
    : IRequestHandler<UpdateContactCommand, ContactOperationResult>
{
    private readonly IContactRepository _contactRepository;
    private readonly IClock _clock;

    public UpdateContactCommandHandler(IContactRepository contactRepository, IClock clock)
    {
        _contactRepository = contactRepository;
        _clock = clock;
    }

    public async Task<ContactOperationResult> Handle(
        UpdateContactCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Name) || string.IsNullOrWhiteSpace(request.Request.Email))
        {
            return ContactOperationResult.InvalidInput();
        }

        var contact = await _contactRepository.GetByIdAsync(
            request.TenantId,
            request.ContactId,
            cancellationToken);

        if (contact is null)
        {
            return ContactOperationResult.NotFound();
        }

        var email = request.Request.Email.Trim().ToLowerInvariant();
        var duplicate = await _contactRepository.EmailExistsAsync(
            request.TenantId,
            contact.CustomerId,
            email,
            request.ContactId,
            cancellationToken);

        if (duplicate)
        {
            return ContactOperationResult.DuplicateEmail();
        }

        if (request.Request.IsPrimary)
        {
            var primaryContacts = await _contactRepository.ListPrimaryByCustomerAsync(
                request.TenantId,
                contact.CustomerId,
                contact.Id,
                cancellationToken);

            foreach (var primaryContact in primaryContacts)
            {
                primaryContact.UnmarkAsPrimary(_clock.UtcNow);
            }
        }

        contact.Update(
            request.Request.Name,
            email,
            request.Request.Phone,
            request.Request.Role,
            request.Request.IsPrimary,
            _clock.UtcNow);
        await _contactRepository.SaveChangesAsync(cancellationToken);

        return ContactOperationResult.Success(ContactResponseMapper.ToResponse(contact));
    }
}
