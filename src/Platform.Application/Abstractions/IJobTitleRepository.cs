using Platform.Domain.Entities;

namespace Platform.Application.Abstractions;

public interface IJobTitleRepository
{
    Task<IReadOnlyCollection<JobTitle>> ListAsync(Guid tenantId, CancellationToken cancellationToken = default);

    Task<JobTitle?> GetByIdAsync(Guid tenantId, Guid jobTitleId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? exceptJobTitleId = null, CancellationToken cancellationToken = default);

    void Add(JobTitle jobTitle);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
