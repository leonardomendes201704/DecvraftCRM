namespace Platform.Provisioning.Abstractions;

public interface ISeedRunner
{
    Task RunAsync(CancellationToken cancellationToken = default);
}
