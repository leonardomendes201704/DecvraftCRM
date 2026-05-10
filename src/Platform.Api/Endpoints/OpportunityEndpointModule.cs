using MediatR;
using Platform.Api.Routing;
using Platform.Api.Security;
using Platform.Application.Abstractions;
using Platform.Application.Opportunities;
using Platform.Domain.Catalog;

namespace Platform.Api.Endpoints;

public sealed class OpportunityEndpointModule : IEndpointModule
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.CustomerOpportunities, async (
            Guid customerId,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(
                request,
                mediator,
                currentUserAccessor,
                cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var opportunities = await mediator.Send(
                new ListOpportunitiesByCustomerQuery(currentUser.TenantId, customerId),
                cancellationToken);

            return Results.Ok(opportunities);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesView)
        .WithName(ApiEndpointNames.OpportunitiesListByCustomer)
        .WithOpenApi();

        app.MapGet(ApiRoutes.OpportunityById, async (
            Guid opportunityId,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(
                request,
                mediator,
                currentUserAccessor,
                cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var opportunity = await mediator.Send(
                new GetOpportunityByIdQuery(currentUser.TenantId, opportunityId),
                cancellationToken);

            return opportunity is null
                ? Results.NotFound()
                : Results.Ok(opportunity);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesView)
        .WithName(ApiEndpointNames.OpportunitiesGetById)
        .WithOpenApi();

        app.MapPost(ApiRoutes.CustomerOpportunities, async (
            Guid customerId,
            CreateOpportunityRequest opportunityRequest,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(
                request,
                mediator,
                currentUserAccessor,
                cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new CreateOpportunityCommand(currentUser.TenantId, customerId, opportunityRequest),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunitiesCreate)
        .WithOpenApi();

        app.MapPut(ApiRoutes.OpportunityById, async (
            Guid opportunityId,
            UpdateOpportunityRequest opportunityRequest,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(
                request,
                mediator,
                currentUserAccessor,
                cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new UpdateOpportunityCommand(currentUser.TenantId, opportunityId, opportunityRequest),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunitiesUpdate)
        .WithOpenApi();

        app.MapPost(ApiRoutes.OpportunityWon, async (
            Guid opportunityId,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(
                request,
                mediator,
                currentUserAccessor,
                cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new MarkOpportunityAsWonCommand(currentUser.TenantId, opportunityId),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunitiesMarkWon)
        .WithOpenApi();

        app.MapPost(ApiRoutes.OpportunityLost, async (
            Guid opportunityId,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(
                request,
                mediator,
                currentUserAccessor,
                cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new MarkOpportunityAsLostCommand(currentUser.TenantId, opportunityId),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunitiesMarkLost)
        .WithOpenApi();

        app.MapPost(ApiRoutes.OpportunityCanceled, async (
            Guid opportunityId,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(
                request,
                mediator,
                currentUserAccessor,
                cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new CancelOpportunityCommand(currentUser.TenantId, opportunityId),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunitiesCancel)
        .WithOpenApi();
    }
}
