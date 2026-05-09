using Platform.Provisioning.Models;

namespace Platform.Provisioning.Abstractions;

public interface IProvisioningService
{
    Task<ProvisioningResult> InstallAsync(InstallRequest request, CancellationToken cancellationToken = default);
}
