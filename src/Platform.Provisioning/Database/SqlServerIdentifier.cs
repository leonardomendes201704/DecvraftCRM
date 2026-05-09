namespace Platform.Provisioning.Database;

public static class SqlServerIdentifier
{
    public static string Quote(string identifier)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);

        return $"[{identifier.Replace("]", "]]", StringComparison.Ordinal)}]";
    }
}
