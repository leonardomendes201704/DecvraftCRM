namespace Platform.Application.Abstractions;

public interface ICurrentTenant
{
    Guid? TenantId { get; }
}
