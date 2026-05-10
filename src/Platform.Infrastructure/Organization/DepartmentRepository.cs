using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Organization;

public sealed class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _dbContext;

    public DepartmentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Department>> ListAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Departments
            .Where(department => department.TenantId == tenantId)
            .OrderBy(department => department.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Department?> GetByIdAsync(
        Guid tenantId,
        Guid departmentId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Departments.SingleOrDefaultAsync(
            department => department.TenantId == tenantId && department.Id == departmentId,
            cancellationToken);
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? exceptDepartmentId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim();

        return _dbContext.Departments.AnyAsync(
            department =>
                department.TenantId == tenantId
                && department.Code == normalizedCode
                && (exceptDepartmentId == null || department.Id != exceptDepartmentId),
            cancellationToken);
    }

    public void Add(Department department)
    {
        _dbContext.Departments.Add(department);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
