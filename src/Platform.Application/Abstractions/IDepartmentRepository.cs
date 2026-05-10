using Platform.Domain.Entities;

namespace Platform.Application.Abstractions;

public interface IDepartmentRepository
{
    Task<IReadOnlyCollection<Department>> ListAsync(Guid tenantId, CancellationToken cancellationToken = default);

    Task<Department?> GetByIdAsync(Guid tenantId, Guid departmentId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? exceptDepartmentId = null, CancellationToken cancellationToken = default);

    void Add(Department department);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
