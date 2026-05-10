namespace Platform.Api.Endpoints;

public static class EndpointModuleExtensions
{
    public static IServiceCollection AddEndpointModules(this IServiceCollection services)
    {
        var moduleTypes = typeof(IEndpointModule).Assembly
            .GetTypes()
            .Where(type =>
                typeof(IEndpointModule).IsAssignableFrom(type)
                && type is { IsAbstract: false, IsInterface: false });

        foreach (var moduleType in moduleTypes)
        {
            services.AddSingleton(typeof(IEndpointModule), moduleType);
        }

        return services;
    }

    public static WebApplication MapEndpointModules(this WebApplication app)
    {
        var modules = app.Services.GetRequiredService<IEnumerable<IEndpointModule>>();

        foreach (var module in modules)
        {
            module.MapEndpoints(app);
        }

        return app;
    }
}
