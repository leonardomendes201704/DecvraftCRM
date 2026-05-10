using System.Xml.Linq;

namespace Platform.ArchitectureTests;

public sealed class HexagonalArchitectureTests
{
    private static readonly IReadOnlyDictionary<string, string[]> AllowedProjectReferences =
        new Dictionary<string, string[]>
        {
            ["Platform.Domain"] = [],
            ["Platform.Application"] = ["Platform.Domain"],
            ["Platform.Modules.Core"] = [],
            ["Platform.Modules.Crm"] = [],
            ["Platform.Modules.Finance"] = [],
            ["Platform.Persistence"] = ["Platform.Domain", "Platform.Application"],
            ["Platform.Infrastructure"] = ["Platform.Application", "Platform.Domain", "Platform.Persistence"],
            ["Platform.Provisioning"] = ["Platform.Domain", "Platform.Application", "Platform.Persistence", "Platform.Infrastructure"],
            ["Platform.Api"] = [
                "Platform.Application",
                "Platform.Infrastructure",
                "Platform.Persistence",
                "Platform.Provisioning",
                "Platform.Modules.Core",
                "Platform.Modules.Crm",
                "Platform.Modules.Finance"
            ],
            ["Platform.WebInstaller"] = [
                "Platform.Application",
                "Platform.Infrastructure",
                "Platform.Persistence",
                "Platform.Provisioning"
            ],
            ["Platform.Worker"] = []
        };

    [Theory]
    [MemberData(nameof(ProjectReferenceRules))]
    public void ProjectReferencesShouldFollowHexagonalBoundaries(
        string projectName,
        IReadOnlyCollection<string> allowedReferences)
    {
        var actualReferences = ReadProjectReferences(projectName);

        var invalidReferences = actualReferences
            .Where(reference => !allowedReferences.Contains(reference))
            .ToArray();

        Assert.Empty(invalidReferences);
    }

    [Fact]
    public void ApplicationPortsShouldRemainInApplicationLayer()
    {
        var abstractionsPath = Path.Combine(GetRepositoryRoot(), "src", "Platform.Application", "Abstractions");

        Assert.True(Directory.Exists(abstractionsPath));
        Assert.NotEmpty(Directory.GetFiles(abstractionsPath, "I*.cs"));
    }

    public static IEnumerable<object[]> ProjectReferenceRules()
    {
        return AllowedProjectReferences.Select(rule => new object[] { rule.Key, rule.Value });
    }

    private static IReadOnlyCollection<string> ReadProjectReferences(string projectName)
    {
        var projectPath = Path.Combine(GetRepositoryRoot(), "src", projectName, $"{projectName}.csproj");
        var project = XDocument.Load(projectPath);

        return project
            .Descendants("ProjectReference")
            .Select(reference => reference.Attribute("Include")?.Value)
            .Where(include => !string.IsNullOrWhiteSpace(include))
            .Select(include => Path.GetFileNameWithoutExtension(include!))
            .OrderBy(reference => reference)
            .ToArray();
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
