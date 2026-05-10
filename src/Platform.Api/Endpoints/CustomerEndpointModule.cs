using Platform.Api.Routing;
using Platform.Api.Security;
using Platform.Application.Abstractions;
using Platform.Application.Customers;
using Platform.Domain.Catalog;
using Platform.Domain.Enums;

namespace Platform.Api.Endpoints;

public sealed class CustomerEndpointModule : IEndpointModule
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Customers, async (
            HttpRequest request,
            IAuthenticationService authenticationService,
            ICustomerService customerService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var customers = await customerService.ListAsync(currentUser.TenantId, cancellationToken);

            return Results.Ok(customers);
        })
        .RequirePermission(KnownPermissions.CrmCustomersView)
        .WithName(ApiEndpointNames.CustomersList)
        .WithOpenApi();

        app.MapGet(ApiRoutes.CustomerById, async (
            Guid customerId,
            HttpRequest request,
            IAuthenticationService authenticationService,
            ICustomerService customerService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var customer = await customerService.GetByIdAsync(currentUser.TenantId, customerId, cancellationToken);

            return customer is null
                ? Results.NotFound()
                : Results.Ok(customer);
        })
        .RequirePermission(KnownPermissions.CrmCustomersView)
        .WithName(ApiEndpointNames.CustomersGetById)
        .WithOpenApi();

        app.MapPost(ApiRoutes.Customers, async (
            CreateCustomerRequest customerRequest,
            HttpRequest request,
            IAuthenticationService authenticationService,
            ICustomerService customerService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await customerService.CreateAsync(currentUser.TenantId, customerRequest, cancellationToken);

            return EndpointResultMapper.ToCustomerWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmCustomersCreate)
        .WithName(ApiEndpointNames.CustomersCreate)
        .WithOpenApi();

        app.MapPut(ApiRoutes.CustomerById, async (
            Guid customerId,
            UpdateCustomerRequest customerRequest,
            HttpRequest request,
            IAuthenticationService authenticationService,
            ICustomerService customerService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await customerService.UpdateAsync(currentUser.TenantId, customerId, customerRequest, cancellationToken);

            return EndpointResultMapper.ToCustomerWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmCustomersUpdate)
        .WithName(ApiEndpointNames.CustomersUpdate)
        .WithOpenApi();

        app.MapDelete(ApiRoutes.CustomerById, async (
            Guid customerId,
            HttpRequest request,
            IAuthenticationService authenticationService,
            ICustomerService customerService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await customerService.DeactivateAsync(currentUser.TenantId, customerId, cancellationToken);

            return result.Status == CustomerOperationStatus.Success
                ? Results.NoContent()
                : EndpointResultMapper.ToCustomerWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmCustomersDelete)
        .WithName(ApiEndpointNames.CustomersDeactivate)
        .WithOpenApi();
    }
}
