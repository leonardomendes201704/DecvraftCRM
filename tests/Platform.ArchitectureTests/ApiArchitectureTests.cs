using Platform.Api.Endpoints;

namespace Platform.ArchitectureTests;

public class ApiArchitectureTests
{
    [Fact]
    public void ApiEndpointsShouldBeMappedByExplicitModules()
    {
        var endpointModules = typeof(IEndpointModule).Assembly
            .GetTypes()
            .Where(type =>
                typeof(IEndpointModule).IsAssignableFrom(type)
                && type is { IsAbstract: false, IsInterface: false })
            .ToArray();

        Assert.Contains(endpointModules, type => type.Name == "AuthEndpointModule");
        Assert.Contains(endpointModules, type => type.Name == "ModuleEndpointModule");
        Assert.Contains(endpointModules, type => type.Name == "CustomerEndpointModule");
        Assert.Contains(endpointModules, type => type.Name == "ContactEndpointModule");
        Assert.Contains(endpointModules, type => type.Name == "OpportunityEndpointModule");
        Assert.Contains(endpointModules, type => type.Name == "FinanceEndpointModule");
    }
}
