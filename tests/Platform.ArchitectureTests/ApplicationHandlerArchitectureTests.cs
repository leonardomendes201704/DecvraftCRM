namespace Platform.ArchitectureTests;

public sealed class ApplicationHandlerArchitectureTests
{
    private static readonly string[] HandlerFolders =
    [
        "Customers",
        "Contacts",
        "Opportunities",
        "Finance"
    ];

    private static readonly string[] ForbiddenHandlerDependencies =
    [
        "ICustomerService",
        "IContactService",
        "IOpportunityService",
        "IFinancialAccountService",
        "IFinancialTransactionService"
    ];

    [Fact]
    public void CrmAndFinanceHandlersShouldDependOnApplicationPorts()
    {
        var applicationPath = Path.Combine(GetRepositoryRoot(), "src", "Platform.Application");

        var handlerFiles = HandlerFolders
            .Select(folder => Path.Combine(applicationPath, folder))
            .SelectMany(folder => Directory.GetFiles(folder, "*Handler.cs"))
            .ToArray();

        var violations = handlerFiles
            .SelectMany(file => ForbiddenHandlerDependencies
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
