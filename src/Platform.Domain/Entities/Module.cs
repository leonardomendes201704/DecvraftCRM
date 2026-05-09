namespace Platform.Domain.Entities;

public sealed class Module
{
    private Module()
    {
    }

    private Module(string name, string slug, string version, bool isCore)
    {
        Id = Guid.NewGuid();
        Name = name;
        Slug = slug;
        Version = version;
        IsCore = isCore;
        IsEnabled = true;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public bool IsCore { get; private set; }
    public bool IsEnabled { get; private set; }

    public static Module Create(string name, string slug, string version, bool isCore)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);
        ArgumentException.ThrowIfNullOrWhiteSpace(version);

        return new Module(name, slug, version, isCore);
    }

    public void Disable() => IsEnabled = false;
}
