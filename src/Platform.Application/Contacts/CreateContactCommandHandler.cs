using MediatR;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;

namespace Platform.Application.Contacts;

public sealed class CreateContactCommandHandler
    : IRequestHandler<CreateContactCommand, ContactOperationResult>
{
    private readonly IContactRepository _contactRepository;
    private readonly IClock _clock;

    public CreateContactCommandHandler(IContactRepository contactRepository, IClock clock)
    {
        _contactRepository = contactRepository;
        _clock = clock;
    }

    public async Task<ContactOperationResult> Handle(
        CreateContactCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Name) || string.IsNullOrWhiteSpace(request.Request.Email))
        {
            return ContactOperationResult.InvalidInput();
        }

        var customerExists = await _contactRepository.CustomerExistsAsync(
            request.TenantId,
            request.CustomerId,
            cancellationToken);

        if (!customerExists)
        {
            return ContactOperationResult.CustomerNotFound();
        }

        var email = request.Request.Email.Trim().ToLowerInvariant();
        var duplicate = await _contactRepository.EmailExistsAsync(
            request.TenantId,
            request.CustomerId,
            email,
            null,
            cancellationToken);

        if (duplicate)
        {
            return ContactOperationResult.DuplicateEmail();
        }

        var contact = Contact.Create(
            request.TenantId,
            request.CustomerId,
            request.Request.Name,
            email,
            request.Request.Phone,
            request.Request.Role,
            _clock.UtcNow);

        if (request.Request.IsPrimary)
        {
            await UnmarkPrimaryContactsAsync(request.TenantId, request.CustomerId, null, cancellationToken);
            contact.MarkAsPrimary(_clock.UtcNow);
        }

        _contactRepository.Add(contact);
        await _contactRepository.SaveChangesAsync(cancellationToken);

        return ContactOperationResult.Success(ContactResponseMapper.ToResponse(contact));
    }

    private async Task UnmarkPrimaryContactsAsync(
        Guid tenantId,
        Guid customerId,
        Guid? exceptContactId,
        CancellationToken cancellationToken)
    {
        var primaryContacts = await _contactRepository.ListPrimaryByCustomerAsync(
            tenantId,
            customerId,
            exceptContactId,
            cancellationToken);

        foreach (var primaryContact in primaryContacts)
        {
            primaryContact.UnmarkAsPrimary(_clock.UtcNow);
        }
    }
}
