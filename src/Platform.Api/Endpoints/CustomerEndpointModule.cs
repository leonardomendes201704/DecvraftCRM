using MediatR;
using Platform.Api.Routing;
using Platform.Api.Security;
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
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, mediator, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var customers = await mediator.Send(new ListCustomersQuery(currentUser.TenantId), cancellationToken);

            return Results.Ok(customers);
        })
        .RequirePermission(KnownPermissions.CrmCustomersView)
        .WithName(ApiEndpointNames.CustomersList)
        .WithOpenApi();

        app.MapGet(ApiRoutes.CustomerById, async (
            Guid customerId,
            HttpRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, mediator, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var customer = await mediator.Send(
                new GetCustomerByIdQuery(currentUser.TenantId, customerId),
                cancellationToken);

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
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, mediator, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new CreateCustomerCommand(currentUser.TenantId, customerRequest),
                cancellationToken);

            return EndpointResultMapper.ToCustomerWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmCustomersCreate)
        .WithName(ApiEndpointNames.CustomersCreate)
        .WithOpenApi();

        app.MapPut(ApiRoutes.CustomerById, async (
            Guid customerId,
            UpdateCustomerRequest customerRequest,
            HttpRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, mediator, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new UpdateCustomerCommand(currentUser.TenantId, customerId, customerRequest),
                cancellationToken);

            return EndpointResultMapper.ToCustomerWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmCustomersUpdate)
        .WithName(ApiEndpointNames.CustomersUpdate)
        .WithOpenApi();

        app.MapDelete(ApiRoutes.CustomerById, async (
            Guid customerId,
            HttpRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, mediator, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new DeactivateCustomerCommand(currentUser.TenantId, customerId),
                cancellationToken);

            return result.Status == CustomerOperationStatus.Success
                ? Results.NoContent()
                : EndpointResultMapper.ToCustomerWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmCustomersDelete)
        .WithName(ApiEndpointNames.CustomersDeactivate)
        .WithOpenApi();
    }

}
