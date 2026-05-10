using Microsoft.Extensions.DependencyInjection;
using Platform.Application.Abstractions;
using Platform.Infrastructure.Auth;
using Platform.Infrastructure.Contacts;
using Platform.Infrastructure.Customers;
using Platform.Infrastructure.Finance;
using Platform.Infrastructure.Modules;
using Platform.Infrastructure.Opportunities;

namespace Platform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IClock, SystemClock>();
        services.AddScoped<IPasswordHashVerifier, PasswordHashVerifier>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPermissionAuthorizationService, PermissionAuthorizationService>();
        services.AddScoped<IModuleCatalogService, ModuleCatalogService>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IOpportunityRepository, OpportunityRepository>();
        services.AddScoped<IFinancialAccountRepository, FinancialAccountRepository>();
        services.AddScoped<IFinancialTransactionRepository, FinancialTransactionRepository>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IOpportunityService, OpportunityService>();
        services.AddScoped<IFinancialAccountService, FinancialAccountService>();
        services.AddScoped<IFinancialTransactionService, FinancialTransactionService>();

        return services;
    }
}
