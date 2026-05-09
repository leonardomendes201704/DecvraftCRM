using Microsoft.Extensions.DependencyInjection;
using Platform.Application.Abstractions;
using Platform.Infrastructure.Auth;
using Platform.Infrastructure.Contacts;
using Platform.Infrastructure.Customers;
using Platform.Infrastructure.Modules;

namespace Platform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHashVerifier, PasswordHashVerifier>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPermissionAuthorizationService, PermissionAuthorizationService>();
        services.AddScoped<IModuleCatalogService, ModuleCatalogService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IContactService, ContactService>();

        return services;
    }
}
