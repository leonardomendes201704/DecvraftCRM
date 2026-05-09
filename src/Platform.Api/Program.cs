using Platform.Api.Routing;
using Platform.Application.Abstractions;
using Platform.Application.Auth;
using Platform.Infrastructure;
using Platform.Persistence;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' was not provided.");

builder.Services.AddPersistence(connectionString);
builder.Services.AddInfrastructure();
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
    var accessToken = ExtractBearerToken(request);
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

app.Run();

static string? ExtractBearerToken(HttpRequest request)
{
    const string bearerPrefix = "Bearer ";

    var authorization = request.Headers.Authorization.ToString();
    if (string.IsNullOrWhiteSpace(authorization) ||
        !authorization.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
    {
        return null;
    }

    return authorization[bearerPrefix.Length..].Trim();
}
