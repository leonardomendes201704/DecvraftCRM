using Platform.Api.Endpoints;
using Platform.Api.Security;
using Platform.Application.Abstractions;
using Platform.Application.Auth;
using Platform.Application.Common.Behaviors;
using Platform.Infrastructure;
using Platform.Persistence;
using Platform.Persistence.Configuration;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString(ConnectionStringNames.DefaultConnection)
    ?? builder.Configuration.GetConnectionString(ConnectionStringNames.LegacyDefault)
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not provided. Configure it with User Secrets or the process environment before starting the API.");

builder.Services.AddPersistence(connectionString);
builder.Services.AddInfrastructure();
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly);
    configuration.AddOpenBehavior(typeof(UnhandledExceptionBehavior<,>));
    configuration.AddOpenBehavior(typeof(RequestLoggingBehavior<,>));
});
builder.Services.AddEndpointModules();
builder.Services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
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

app.MapEndpointModules();

app.Run();
