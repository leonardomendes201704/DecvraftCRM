namespace Platform.Domain.Entities;

public sealed class SystemConfiguration
{
    private SystemConfiguration()
    {
    }

    private SystemConfiguration(string key, string value, bool isSecret, DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        Key = key;
        Value = value;
        IsSecret = isSecret;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public string Key { get; private set; } = string.Empty;
    public string Value { get; private set; } = string.Empty;
    public bool IsSecret { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public static SystemConfiguration Create(string key, string value, bool isSecret, DateTimeOffset createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        return new SystemConfiguration(key, value, isSecret, createdAt);
    }

    public void UpdateValue(string value, DateTimeOffset updatedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = value;
        UpdatedAt = updatedAt;
    }
}
