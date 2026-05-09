using Platform.Api.Routing;
using Platform.Api.Security;
using Platform.Application.Abstractions;
using Platform.Application.Auth;
using Platform.Application.Customers;
using Platform.Domain.Catalog;
using Platform.Domain.Enums;
using Platform.Infrastructure;
using Platform.Persistence;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' was not provided.");

builder.Services.AddPersistence(connectionString);
builder.Services.AddInfrastructure();
builder.Services.AddScoped<PermissionEndpointFilter>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost(ApiRoutes.AuthLogin, async (
    LoginRequest request,
    IAuthenticationService authenticationService,
    CancellationToken cancellationToken) =>
{
    var result = await authenticationService.LoginAsync(request, cancellationToken);

    return result.Succeeded
        ? Results.Ok(result.Login)
        : Results.Unauthorized();
})
.WithName(ApiEndpointNames.Login)
.WithOpenApi();

app.MapGet(ApiRoutes.Me, async (
    HttpRequest request,
    IAuthenticationService authenticationService,
    CancellationToken cancellationToken) =>
{
    var accessToken = BearerTokenReader.Read(request);
    if (accessToken is null)
    {
        return Results.Unauthorized();
    }

    var currentUser = await authenticationService.GetCurrentUserAsync(accessToken, cancellationToken);

    return currentUser is not null
        ? Results.Ok(currentUser)
        : Results.Unauthorized();
})
.WithName(ApiEndpointNames.Me)
.WithOpenApi();

app.MapGet(ApiRoutes.Modules, async (
    HttpRequest request,
    IAuthenticationService authenticationService,
    IModuleCatalogService moduleCatalogService,
    CancellationToken cancellationToken) =>
{
    var accessToken = BearerTokenReader.Read(request);
    if (accessToken is null)
    {
        return Results.Unauthorized();
    }

    var currentUser = await authenticationService.GetCurrentUserAsync(accessToken, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var modules = await moduleCatalogService.GetModulesAsync(currentUser.TenantId, cancellationToken);

    return Results.Ok(modules);
})
.RequirePermission(KnownPermissions.CoreModulesView)
.WithName(ApiEndpointNames.Modules)
.WithOpenApi();

app.MapGet(ApiRoutes.Customers, async (
    HttpRequest request,
    IAuthenticationService authenticationService,
    ICustomerService customerService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
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
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
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
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await customerService.CreateAsync(currentUser.TenantId, customerRequest, cancellationToken);

    return ToCustomerWriteResult(result);
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
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await customerService.UpdateAsync(currentUser.TenantId, customerId, customerRequest, cancellationToken);

    return ToCustomerWriteResult(result);
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
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await customerService.DeactivateAsync(currentUser.TenantId, customerId, cancellationToken);

    return result.Status == CustomerOperationStatus.Success
        ? Results.NoContent()
        : ToCustomerWriteResult(result);
})
.RequirePermission(KnownPermissions.CrmCustomersDelete)
.WithName(ApiEndpointNames.CustomersDeactivate)
.WithOpenApi();

app.MapGet(ApiRoutes.SystemPermissions, () => KnownPermissions.All)
.RequirePermission(KnownPermissions.CoreSystemView)
.WithName(ApiEndpointNames.SystemPermissions)
.WithOpenApi();

app.Run();

static async Task<CurrentUserResponse?> ResolveCurrentUserAsync(
    HttpRequest request,
    IAuthenticationService authenticationService,
    CancellationToken cancellationToken)
{
    var accessToken = BearerTokenReader.Read(request);

    return accessToken is null
        ? null
        : await authenticationService.GetCurrentUserAsync(accessToken, cancellationToken);
}

static IResult ToCustomerWriteResult(CustomerOperationResult result)
{
    return result.Status switch
    {
        CustomerOperationStatus.Success => Results.Ok(result.Customer),
        CustomerOperationStatus.NotFound => Results.NotFound(),
        CustomerOperationStatus.DuplicateDocument => Results.Conflict(),
        CustomerOperationStatus.InvalidInput => Results.BadRequest(),
        _ => Results.BadRequest()
    };
}
