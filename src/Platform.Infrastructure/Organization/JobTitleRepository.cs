using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Organization;

public sealed class JobTitleRepository : IJobTitleRepository
{
    private readonly AppDbContext _dbContext;

    public JobTitleRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<JobTitle>> ListAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.JobTitles
            .Where(jobTitle => jobTitle.TenantId == tenantId)
            .OrderBy(jobTitle => jobTitle.Level)
            .ThenBy(jobTitle => jobTitle.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<JobTitle?> GetByIdAsync(
        Guid tenantId,
        Guid jobTitleId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.JobTitles.SingleOrDefaultAsync(
            jobTitle => jobTitle.TenantId == tenantId && jobTitle.Id == jobTitleId,
            cancellationToken);
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? exceptJobTitleId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim();

        return _dbContext.JobTitles.AnyAsync(
            jobTitle =>
                jobTitle.TenantId == tenantId
                && jobTitle.Code == normalizedCode
                && (exceptJobTitleId == null || jobTitle.Id != exceptJobTitleId),
            cancellationToken);
    }

    public void Add(JobTitle jobTitle)
    {
        _dbContext.JobTitles.Add(jobTitle);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
