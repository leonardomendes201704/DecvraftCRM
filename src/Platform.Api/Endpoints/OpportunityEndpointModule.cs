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
            IAuthenticationService authenticationService,
            IOpportunityService opportunityService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var opportunities = await opportunityService.ListByCustomerAsync(currentUser.TenantId, customerId, cancellationToken);

            return Results.Ok(opportunities);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesView)
        .WithName(ApiEndpointNames.OpportunitiesListByCustomer)
        .WithOpenApi();

        app.MapGet(ApiRoutes.OpportunityById, async (
            Guid opportunityId,
            HttpRequest request,
            IAuthenticationService authenticationService,
            IOpportunityService opportunityService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var opportunity = await opportunityService.GetByIdAsync(currentUser.TenantId, opportunityId, cancellationToken);

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
            IAuthenticationService authenticationService,
            IOpportunityService opportunityService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await opportunityService.CreateAsync(currentUser.TenantId, customerId, opportunityRequest, cancellationToken);

            return EndpointResultMapper.ToOpportunityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunitiesCreate)
        .WithOpenApi();

        app.MapPut(ApiRoutes.OpportunityById, async (
            Guid opportunityId,
            UpdateOpportunityRequest opportunityRequest,
            HttpRequest request,
            IAuthenticationService authenticationService,
            IOpportunityService opportunityService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await opportunityService.UpdateAsync(currentUser.TenantId, opportunityId, opportunityRequest, cancellationToken);

            return EndpointResultMapper.ToOpportunityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunitiesUpdate)
        .WithOpenApi();

        app.MapPost(ApiRoutes.OpportunityWon, async (
            Guid opportunityId,
            HttpRequest request,
            IAuthenticationService authenticationService,
            IOpportunityService opportunityService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await opportunityService.MarkAsWonAsync(currentUser.TenantId, opportunityId, cancellationToken);

            return EndpointResultMapper.ToOpportunityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunitiesMarkWon)
        .WithOpenApi();

        app.MapPost(ApiRoutes.OpportunityLost, async (
            Guid opportunityId,
            HttpRequest request,
            IAuthenticationService authenticationService,
            IOpportunityService opportunityService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await opportunityService.MarkAsLostAsync(currentUser.TenantId, opportunityId, cancellationToken);

            return EndpointResultMapper.ToOpportunityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunitiesMarkLost)
        .WithOpenApi();

        app.MapPost(ApiRoutes.OpportunityCanceled, async (
            Guid opportunityId,
            HttpRequest request,
            IAuthenticationService authenticationService,
            IOpportunityService opportunityService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await opportunityService.CancelAsync(currentUser.TenantId, opportunityId, cancellationToken);

            return EndpointResultMapper.ToOpportunityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunitiesCancel)
        .WithOpenApi();
    }
}
