using Platform.Domain.Entities;

namespace Platform.Application.Abstractions;

public interface IEmployeeRepository
{
    Task<IReadOnlyCollection<Employee>> ListAsync(Guid tenantId, CancellationToken cancellationToken = default);

    Task<Employee?> GetByIdAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken = default);

    Task<Employee?> GetByApplicationUserIdAsync(
        Guid tenantId,
        Guid applicationUserId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Employee>> ListSubordinatesAsync(
        Guid tenantId,
        Guid managerEmployeeId,
        CancellationToken cancellationToken = default);

    Task<bool> DepartmentExistsAsync(Guid tenantId, Guid departmentId, CancellationToken cancellationToken = default);

    Task<bool> JobTitleExistsAsync(Guid tenantId, Guid jobTitleId, CancellationToken cancellationToken = default);

    Task<bool> EmployeeExistsAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken = default);

    Task<bool> ApplicationUserExistsAsync(Guid tenantId, Guid applicationUserId, CancellationToken cancellationToken = default);

    Task<bool> ApplicationUserLinkedAsync(
        Guid tenantId,
        Guid applicationUserId,
        Guid? exceptEmployeeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCorporateEmailAsync(
        Guid tenantId,
        string corporateEmail,
        Guid? exceptEmployeeId = null,
        CancellationToken cancellationToken = default);

    void Add(Employee employee);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
