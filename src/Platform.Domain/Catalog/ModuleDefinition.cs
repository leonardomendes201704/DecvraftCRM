namespace Platform.Domain.Catalog;

public sealed record ModuleDefinition(string Slug, string Name, string Version, bool IsCore);
