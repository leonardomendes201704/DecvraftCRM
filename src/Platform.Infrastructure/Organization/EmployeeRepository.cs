using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Entities;
using Platform.Persistence;

namespace Platform.Infrastructure.Organization;

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _dbContext;

    public EmployeeRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Employee>> ListAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Employees
            .Where(employee => employee.TenantId == tenantId)
            .OrderBy(employee => employee.FullName)
            .ToListAsync(cancellationToken);
    }

    public Task<Employee?> GetByIdAsync(
        Guid tenantId,
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Employees.SingleOrDefaultAsync(
            employee => employee.TenantId == tenantId && employee.Id == employeeId,
            cancellationToken);
    }

    public async Task<IReadOnlyCollection<Employee>> ListSubordinatesAsync(
        Guid tenantId,
        Guid managerEmployeeId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Employees
            .Where(employee => employee.TenantId == tenantId && employee.ManagerEmployeeId == managerEmployeeId)
            .OrderBy(employee => employee.FullName)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> DepartmentExistsAsync(
        Guid tenantId,
        Guid departmentId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Departments.AnyAsync(
            department => department.TenantId == tenantId && department.Id == departmentId,
            cancellationToken);
    }

    public Task<bool> JobTitleExistsAsync(
        Guid tenantId,
        Guid jobTitleId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.JobTitles.AnyAsync(
            jobTitle => jobTitle.TenantId == tenantId && jobTitle.Id == jobTitleId,
            cancellationToken);
    }

    public Task<bool> EmployeeExistsAsync(
        Guid tenantId,
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Employees.AnyAsync(
            employee => employee.TenantId == tenantId && employee.Id == employeeId,
            cancellationToken);
    }

    public Task<bool> ExistsByCorporateEmailAsync(
        Guid tenantId,
        string corporateEmail,
        Guid? exceptEmployeeId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = corporateEmail.Trim().ToLowerInvariant();

        return _dbContext.Employees.AnyAsync(
            employee =>
                employee.TenantId == tenantId
                && employee.CorporateEmail == normalizedEmail
                && (exceptEmployeeId == null || employee.Id != exceptEmployeeId),
            cancellationToken);
    }

    public void Add(Employee employee)
    {
        _dbContext.Employees.Add(employee);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
