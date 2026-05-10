using MediatR;
using Platform.Api.Routing;
using Platform.Api.Security;
using Platform.Application.Abstractions;
using Platform.Application.Auth;
using Platform.Application.Organization;
using Platform.Domain.Catalog;
using Platform.Domain.Enums;

namespace Platform.Api.Endpoints;

public sealed class OrganizationEndpointModule : IEndpointModule
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Departments, async (
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var departments = await mediator.Send(new ListDepartmentsQuery(currentUser.TenantId), cancellationToken);

            return Results.Ok(departments);
        })
        .RequirePermission(KnownPermissions.CoreDepartmentsView)
        .WithName(ApiEndpointNames.DepartmentsList)
        .WithOpenApi();

        app.MapGet(ApiRoutes.DepartmentById, async (
            Guid departmentId,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var department = await mediator.Send(
                new GetDepartmentByIdQuery(currentUser.TenantId, departmentId),
                cancellationToken);

            return department is null ? Results.NotFound() : Results.Ok(department);
        })
        .RequirePermission(KnownPermissions.CoreDepartmentsView)
        .WithName(ApiEndpointNames.DepartmentsGetById)
        .WithOpenApi();

        app.MapPost(ApiRoutes.Departments, async (
            CreateDepartmentRequest departmentRequest,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new CreateDepartmentCommand(currentUser.TenantId, departmentRequest),
                cancellationToken);

            return EndpointResultMapper.ToDepartmentWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CoreDepartmentsManage)
        .WithName(ApiEndpointNames.DepartmentsCreate)
        .WithOpenApi();

        app.MapPut(ApiRoutes.DepartmentById, async (
            Guid departmentId,
            UpdateDepartmentRequest departmentRequest,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new UpdateDepartmentCommand(currentUser.TenantId, departmentId, departmentRequest),
                cancellationToken);

            return EndpointResultMapper.ToDepartmentWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CoreDepartmentsManage)
        .WithName(ApiEndpointNames.DepartmentsUpdate)
        .WithOpenApi();

        app.MapDelete(ApiRoutes.DepartmentById, async (
            Guid departmentId,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new DeactivateDepartmentCommand(currentUser.TenantId, departmentId),
                cancellationToken);

            return result.Status == OrganizationOperationStatus.Success
                ? Results.NoContent()
                : EndpointResultMapper.ToDepartmentWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CoreDepartmentsManage)
        .WithName(ApiEndpointNames.DepartmentsDeactivate)
        .WithOpenApi();

        MapJobTitleEndpoints(app);
        MapEmployeeEndpoints(app);
    }

    private static void MapJobTitleEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.JobTitles, async (
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var jobTitles = await mediator.Send(new ListJobTitlesQuery(currentUser.TenantId), cancellationToken);

            return Results.Ok(jobTitles);
        })
        .RequirePermission(KnownPermissions.CoreJobTitlesView)
        .WithName(ApiEndpointNames.JobTitlesList)
        .WithOpenApi();

        app.MapGet(ApiRoutes.JobTitleById, async (
            Guid jobTitleId,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var jobTitle = await mediator.Send(
                new GetJobTitleByIdQuery(currentUser.TenantId, jobTitleId),
                cancellationToken);

            return jobTitle is null ? Results.NotFound() : Results.Ok(jobTitle);
        })
        .RequirePermission(KnownPermissions.CoreJobTitlesView)
        .WithName(ApiEndpointNames.JobTitlesGetById)
        .WithOpenApi();

        app.MapPost(ApiRoutes.JobTitles, async (
            CreateJobTitleRequest jobTitleRequest,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new CreateJobTitleCommand(currentUser.TenantId, jobTitleRequest),
                cancellationToken);

            return EndpointResultMapper.ToJobTitleWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CoreJobTitlesManage)
        .WithName(ApiEndpointNames.JobTitlesCreate)
        .WithOpenApi();

        app.MapPut(ApiRoutes.JobTitleById, async (
            Guid jobTitleId,
            UpdateJobTitleRequest jobTitleRequest,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new UpdateJobTitleCommand(currentUser.TenantId, jobTitleId, jobTitleRequest),
                cancellationToken);

            return EndpointResultMapper.ToJobTitleWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CoreJobTitlesManage)
        .WithName(ApiEndpointNames.JobTitlesUpdate)
        .WithOpenApi();

        app.MapDelete(ApiRoutes.JobTitleById, async (
            Guid jobTitleId,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new DeactivateJobTitleCommand(currentUser.TenantId, jobTitleId),
                cancellationToken);

            return result.Status == OrganizationOperationStatus.Success
                ? Results.NoContent()
                : EndpointResultMapper.ToJobTitleWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CoreJobTitlesManage)
        .WithName(ApiEndpointNames.JobTitlesDeactivate)
        .WithOpenApi();
    }

    private static void MapEmployeeEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Employees, async (
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var employees = await mediator.Send(new ListEmployeesQuery(currentUser.TenantId), cancellationToken);

            return Results.Ok(employees);
        })
        .RequirePermission(KnownPermissions.CoreEmployeesView)
        .WithName(ApiEndpointNames.EmployeesList)
        .WithOpenApi();

        app.MapGet(ApiRoutes.EmployeeById, async (
            Guid employeeId,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var employee = await mediator.Send(
                new GetEmployeeByIdQuery(currentUser.TenantId, employeeId),
                cancellationToken);

            return employee is null ? Results.NotFound() : Results.Ok(employee);
        })
        .RequirePermission(KnownPermissions.CoreEmployeesView)
        .WithName(ApiEndpointNames.EmployeesGetById)
        .WithOpenApi();

        app.MapGet(ApiRoutes.EmployeeSubordinates, async (
            Guid employeeId,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var employees = await mediator.Send(
                new ListEmployeeSubordinatesQuery(currentUser.TenantId, employeeId),
                cancellationToken);

            return Results.Ok(employees);
        })
        .RequirePermission(KnownPermissions.CoreHierarchyView)
        .WithName(ApiEndpointNames.EmployeesSubordinates)
        .WithOpenApi();

        app.MapPost(ApiRoutes.Employees, async (
            CreateEmployeeRequest employeeRequest,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new CreateEmployeeCommand(currentUser.TenantId, employeeRequest),
                cancellationToken);

            return EndpointResultMapper.ToEmployeeWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CoreEmployeesManage)
        .WithName(ApiEndpointNames.EmployeesCreate)
        .WithOpenApi();

        app.MapPut(ApiRoutes.EmployeeById, async (
            Guid employeeId,
            UpdateEmployeeRequest employeeRequest,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new UpdateEmployeeCommand(currentUser.TenantId, employeeId, employeeRequest),
                cancellationToken);

            return EndpointResultMapper.ToEmployeeWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CoreEmployeesManage)
        .WithName(ApiEndpointNames.EmployeesUpdate)
        .WithOpenApi();

        app.MapDelete(ApiRoutes.EmployeeById, async (
            Guid employeeId,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new DeactivateEmployeeCommand(currentUser.TenantId, employeeId),
                cancellationToken);

            return result.Status == OrganizationOperationStatus.Success
                ? Results.NoContent()
                : EndpointResultMapper.ToEmployeeWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CoreEmployeesManage)
        .WithName(ApiEndpointNames.EmployeesDeactivate)
        .WithOpenApi();

        app.MapPost(ApiRoutes.EmployeeLinkUser, async (
            Guid employeeId,
            LinkEmployeeUserRequest linkRequest,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new LinkEmployeeUserCommand(currentUser.TenantId, employeeId, linkRequest),
                cancellationToken);

            return EndpointResultMapper.ToEmployeeLinkWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CoreEmployeesManage)
        .WithName(ApiEndpointNames.EmployeesLinkUser)
        .WithOpenApi();

        app.MapPost(ApiRoutes.EmployeeUnlinkUser, async (
            Guid employeeId,
            HttpRequest request,
            IMediator mediator,
            ICurrentUserAccessor currentUserAccessor,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await ResolveCurrentUserAsync(request, mediator, currentUserAccessor, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(
                new UnlinkEmployeeUserCommand(currentUser.TenantId, employeeId),
                cancellationToken);

            return EndpointResultMapper.ToEmployeeLinkWriteResult(result);
        })
        .RequirePermission(KnownPermissions.CoreEmployeesManage)
        .WithName(ApiEndpointNames.EmployeesUnlinkUser)
        .WithOpenApi();
    }

    private static Task<CurrentUserResponse?> ResolveCurrentUserAsync(
        HttpRequest request,
        IMediator mediator,
        ICurrentUserAccessor currentUserAccessor,
        CancellationToken cancellationToken)
    {
        return EndpointUserResolver.ResolveCurrentUserAsync(
            request,
            mediator,
            currentUserAccessor,
            cancellationToken);
    }
}
