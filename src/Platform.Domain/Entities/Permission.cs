namespace Platform.Domain.Entities;

public sealed class Permission
{
    private Permission()
    {
    }

    private Permission(string key, string description, string module)
    {
        Id = Guid.NewGuid();
        Key = key;
        Description = description;
        Module = module;
    }

    public Guid Id { get; private set; }
    public string Key { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Module { get; private set; } = string.Empty;

    public static Permission Create(string key, string description, string module)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(module);

        return new Permission(key, description, module);
    }
}
