using Platform.Api.Endpoints;
using Platform.Api.Security;
using Platform.Infrastructure;
using Platform.Persistence;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' was not provided.");

builder.Services.AddPersistence(connectionString);
builder.Services.AddInfrastructure();
builder.Services.AddEndpointModules();
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
