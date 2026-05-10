using Platform.Api.Routing;
using Platform.Api.Security;
using Platform.Application.Abstractions;
using Platform.Application.Contacts;
using Platform.Domain.Catalog;
using Platform.Domain.Enums;

namespace Platform.Api.Endpoints;

public sealed class ContactEndpointModule : IEndpointModule
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.CustomerContacts, async (
            Guid customerId,
            HttpRequest request,
            IAuthenticationService authenticationService,
            IContactService contactService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var contacts = await contactService.ListByCustomerAsync(currentUser.TenantId, customerId, cancellationToken);

            return Results.Ok(contacts);
        })
        .RequirePermission(KnownPermissions.CrmCustomersView)
        .WithName(ApiEndpointNames.ContactsListByCustomer)
        .WithOpenApi();

        app.MapGet(ApiRoutes.ContactById, async (
            Guid contactId,
            HttpRequest request,
            IAuthenticationService authenticationService,
            IContactService contactService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var contact = await contactService.GetByIdAsync(currentUser.TenantId, contactId, cancellationToken);

            return contact is null
                ? Results.NotFound()
                : Results.Ok(contact);
        })
        .RequirePermission(KnownPermissions.CrmCustomersView)
        .WithName(ApiEndpointNames.ContactsGetById)
        .WithOpenApi();

        app.MapPost(ApiRoutes.CustomerContacts, async (
            Guid customerId,
            CreateContactRequest contactRequest,
            HttpRequest request,
            IAuthenticationService authenticationService,
            IContactService contactService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await contactService.CreateAsync(currentUser.TenantId, customerId, contactRequest, cancellationToken);

            return EndpointResultMapper.ToContactWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmCustomersCreate)
        .WithName(ApiEndpointNames.ContactsCreate)
        .WithOpenApi();

        app.MapPut(ApiRoutes.ContactById, async (
            Guid contactId,
            UpdateContactRequest contactRequest,
            HttpRequest request,
            IAuthenticationService authenticationService,
            IContactService contactService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await contactService.UpdateAsync(currentUser.TenantId, contactId, contactRequest, cancellationToken);

            return EndpointResultMapper.ToContactWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmCustomersUpdate)
        .WithName(ApiEndpointNames.ContactsUpdate)
        .WithOpenApi();

        app.MapDelete(ApiRoutes.ContactById, async (
            Guid contactId,
            HttpRequest request,
            IAuthenticationService authenticationService,
            IContactService contactService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await contactService.DeleteAsync(currentUser.TenantId, contactId, cancellationToken);

            return result.Status == ContactOperationStatus.Success
                ? Results.NoContent()
                : EndpointResultMapper.ToContactWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmCustomersDelete)
        .WithName(ApiEndpointNames.ContactsDelete)
        .WithOpenApi();
    }
}
