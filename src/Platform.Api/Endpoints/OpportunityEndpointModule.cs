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
            Guid? ownerEmployeeId,
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
                new ListOpportunitiesByCustomerQuery(currentUser.TenantId, customerId, ownerEmployeeId),
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

        app.MapGet(ApiRoutes.OpportunityStages, async (
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

            var stages = await mediator.Send(new ListOpportunityStagesQuery(currentUser.TenantId), cancellationToken);

            return Results.Ok(stages);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesView)
        .WithName(ApiEndpointNames.OpportunityStagesList)
        .WithOpenApi();

        app.MapPost(ApiRoutes.OpportunityStages, async (
            CreateOpportunityStageRequest stageRequest,
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
                new CreateOpportunityStageCommand(currentUser.TenantId, stageRequest),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityStageWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunityStagesCreate)
        .WithOpenApi();

        app.MapPut(ApiRoutes.OpportunityStageById, async (
            Guid stageId,
            UpdateOpportunityStageRequest stageRequest,
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
                new UpdateOpportunityStageCommand(currentUser.TenantId, stageId, stageRequest),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityStageWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunityStagesUpdate)
        .WithOpenApi();

        app.MapDelete(ApiRoutes.OpportunityStageById, async (
            Guid stageId,
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
                new DeactivateOpportunityStageCommand(currentUser.TenantId, stageId),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityStageWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunityStagesDeactivate)
        .WithOpenApi();

        app.MapGet(ApiRoutes.OpportunityStageOpportunities, async (
            Guid stageId,
            Guid? ownerEmployeeId,
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
                new ListOpportunitiesByStageQuery(currentUser.TenantId, stageId, ownerEmployeeId),
                cancellationToken);

            return Results.Ok(opportunities);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesView)
        .WithName(ApiEndpointNames.OpportunityStagesListOpportunities)
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

        app.MapPut(ApiRoutes.OpportunityStageMove, async (
            Guid opportunityId,
            MoveOpportunityStageRequest moveRequest,
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
                new MoveOpportunityStageCommand(currentUser.TenantId, opportunityId, moveRequest),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunitiesMoveStage)
        .WithOpenApi();

        app.MapPut(ApiRoutes.OpportunityOwner, async (
            Guid opportunityId,
            AssignOpportunityOwnerRequest ownerRequest,
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
                new AssignOpportunityOwnerCommand(currentUser.TenantId, opportunityId, ownerRequest),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunitiesAssignOwner)
        .WithOpenApi();

        app.MapGet(ApiRoutes.OpportunityActivities, async (
            Guid opportunityId,
            Guid? ownerEmployeeId,
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

            var activities = await mediator.Send(
                new ListOpportunityActivitiesQuery(currentUser.TenantId, opportunityId, ownerEmployeeId),
                cancellationToken);

            return Results.Ok(activities);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesView)
        .WithName(ApiEndpointNames.OpportunityActivitiesList)
        .WithOpenApi();

        app.MapGet(ApiRoutes.OpportunityActivitiesOverdue, async (
            Guid? ownerEmployeeId,
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

            var activities = await mediator.Send(
                new ListOverdueOpportunityActivitiesQuery(currentUser.TenantId, ownerEmployeeId),
                cancellationToken);

            return Results.Ok(activities);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesView)
        .WithName(ApiEndpointNames.OpportunityActivitiesOverdue)
        .WithOpenApi();

        app.MapGet(ApiRoutes.OpportunityActivitiesUpcoming, async (
            int? days,
            Guid? ownerEmployeeId,
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

            var activities = await mediator.Send(
                new ListUpcomingOpportunityActivitiesQuery(currentUser.TenantId, days ?? 7, ownerEmployeeId),
                cancellationToken);

            return Results.Ok(activities);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesView)
        .WithName(ApiEndpointNames.OpportunityActivitiesUpcoming)
        .WithOpenApi();

        app.MapPost(ApiRoutes.OpportunityActivities, async (
            Guid opportunityId,
            CreateOpportunityActivityRequest activityRequest,
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
                new CreateOpportunityActivityCommand(currentUser.TenantId, opportunityId, activityRequest),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityActivityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunityActivitiesCreate)
        .WithOpenApi();

        app.MapPut(ApiRoutes.OpportunityActivityById, async (
            Guid activityId,
            UpdateOpportunityActivityRequest activityRequest,
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
                new UpdateOpportunityActivityCommand(currentUser.TenantId, activityId, activityRequest),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityActivityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunityActivitiesUpdate)
        .WithOpenApi();

        app.MapPut(ApiRoutes.OpportunityActivityOwner, async (
            Guid activityId,
            AssignOpportunityActivityOwnerRequest ownerRequest,
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
                new AssignOpportunityActivityOwnerCommand(currentUser.TenantId, activityId, ownerRequest),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityActivityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunityActivitiesAssignOwner)
        .WithOpenApi();

        app.MapPost(ApiRoutes.OpportunityActivityComplete, async (
            Guid activityId,
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
                new CompleteOpportunityActivityCommand(currentUser.TenantId, activityId),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityActivityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunityActivitiesComplete)
        .WithOpenApi();

        app.MapPost(ApiRoutes.OpportunityActivityCancel, async (
            Guid activityId,
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
                new CancelOpportunityActivityCommand(currentUser.TenantId, activityId),
                cancellationToken);

            return EndpointResultMapper.ToOpportunityActivityWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesManage)
        .WithName(ApiEndpointNames.OpportunityActivitiesCancel)
        .WithOpenApi();

        app.MapGet(ApiRoutes.OpportunityHistory, async (
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

            var history = await mediator.Send(
                new ListOpportunityHistoryQuery(currentUser.TenantId, opportunityId),
                cancellationToken);

            return Results.Ok(history);
        })
        .RequirePermission(KnownPermissions.CrmOpportunitiesView)
        .WithName(ApiEndpointNames.OpportunityHistoryList)
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
