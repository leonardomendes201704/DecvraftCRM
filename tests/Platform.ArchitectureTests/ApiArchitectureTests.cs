using Platform.Api.Endpoints;

namespace Platform.ArchitectureTests;

public class ApiArchitectureTests
{
    private static readonly string[] ForbiddenEndpointDependencies =
    [
        "IAuthenticationService",
        "ICustomerService",
        "IContactService",
        "IOpportunityService",
        "IFinancialAccountService",
        "IFinancialTransactionService",
        "IModuleCatalogService"
    ];

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

    [Fact]
    public void EndpointModulesShouldUseMediatorInsteadOfApplicationServicesDirectly()
    {
        var endpointPath = Path.Combine(GetRepositoryRoot(), "src", "Platform.Api", "Endpoints");
        var endpointFiles = Directory.GetFiles(endpointPath, "*EndpointModule.cs");

        var violations = endpointFiles
            .SelectMany(file => ForbiddenEndpointDependencies
                .Where(dependency => File.ReadAllText(file).Contains(dependency, StringComparison.Ordinal))
                .Select(dependency => $"{Path.GetFileName(file)} references {dependency}"))
            .ToArray();

        Assert.Empty(violations);
    }

    private static string GetRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, "src"))
                && Directory.Exists(Path.Combine(directory.FullName, "tests")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException("Repository root could not be resolved.");
    }
}
