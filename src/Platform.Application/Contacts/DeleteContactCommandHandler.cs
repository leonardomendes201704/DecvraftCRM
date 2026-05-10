using MediatR;
using Platform.Application.Abstractions;

namespace Platform.Application.Contacts;

public sealed class DeleteContactCommandHandler
    : IRequestHandler<DeleteContactCommand, ContactOperationResult>
{
    private readonly IContactRepository _contactRepository;

    public DeleteContactCommandHandler(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<ContactOperationResult> Handle(
        DeleteContactCommand request,
        CancellationToken cancellationToken)
    {
        var contact = await _contactRepository.GetByIdAsync(request.TenantId, request.ContactId, cancellationToken);

        if (contact is null)
        {
            return ContactOperationResult.NotFound();
        }

        _contactRepository.Remove(contact);
        await _contactRepository.SaveChangesAsync(cancellationToken);

        return ContactOperationResult.Success(ContactResponseMapper.ToResponse(contact));
    }
}
