using Platform.Domain.Common;
using Platform.Domain.Enums;

namespace Platform.Domain.Entities;

public sealed class Employee : ITenantEntity
{
    private Employee()
    {
    }

    private Employee(
        Guid tenantId,
        Guid? applicationUserId,
        Guid? departmentId,
        Guid? jobTitleId,
        Guid? managerEmployeeId,
        string fullName,
        string? document,
        string? corporateEmail,
        string? phone,
        DateOnly? hireDate,
        DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        ApplicationUserId = applicationUserId;
        DepartmentId = departmentId;
        JobTitleId = jobTitleId;
        ManagerEmployeeId = managerEmployeeId;
        FullName = fullName;
        Document = document;
        CorporateEmail = corporateEmail;
        Phone = phone;
        HireDate = hireDate;
        Status = EmployeeStatus.Active;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid? ApplicationUserId { get; private set; }
    public Guid? DepartmentId { get; private set; }
    public Guid? JobTitleId { get; private set; }
    public Guid? ManagerEmployeeId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string? Document { get; private set; }
    public string? CorporateEmail { get; private set; }
    public string? Phone { get; private set; }
    public DateOnly? HireDate { get; private set; }
    public DateOnly? TerminationDate { get; private set; }
    public EmployeeStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public static Employee Create(
        Guid tenantId,
        Guid? applicationUserId,
        Guid? departmentId,
        Guid? jobTitleId,
        Guid? managerEmployeeId,
        string fullName,
        string? document,
        string? corporateEmail,
        string? phone,
        DateOnly? hireDate,
        DateTimeOffset createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);

        var employee = new Employee(
            tenantId,
            applicationUserId,
            departmentId,
            jobTitleId,
            managerEmployeeId,
            fullName.Trim(),
            Normalize(document),
            NormalizeEmail(corporateEmail),
            Normalize(phone),
            hireDate,
            createdAt);

        employee.EnsureManagerIsNotSelf(managerEmployeeId);

        return employee;
    }

    public void UpdateProfile(
        Guid? departmentId,
        Guid? jobTitleId,
        Guid? managerEmployeeId,
        string fullName,
        string? document,
        string? corporateEmail,
        string? phone,
        DateOnly? hireDate,
        DateTimeOffset updatedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        EnsureManagerIsNotSelf(managerEmployeeId);

        DepartmentId = departmentId;
        JobTitleId = jobTitleId;
        ManagerEmployeeId = managerEmployeeId;
        FullName = fullName.Trim();
        Document = Normalize(document);
        CorporateEmail = NormalizeEmail(corporateEmail);
        Phone = Normalize(phone);
        HireDate = hireDate;
        UpdatedAt = updatedAt;
    }

    public void LinkUser(Guid applicationUserId, DateTimeOffset updatedAt)
    {
        ApplicationUserId = applicationUserId;
        UpdatedAt = updatedAt;
    }

    public void UnlinkUser(DateTimeOffset updatedAt)
    {
        ApplicationUserId = null;
        UpdatedAt = updatedAt;
    }

    public void ChangeStatus(EmployeeStatus status, DateTimeOffset updatedAt)
    {
        Status = status;
        UpdatedAt = updatedAt;
    }

    public void Terminate(DateOnly terminationDate, DateTimeOffset updatedAt)
    {
        TerminationDate = terminationDate;
        Status = EmployeeStatus.Terminated;
        UpdatedAt = updatedAt;
    }

    private void EnsureManagerIsNotSelf(Guid? managerEmployeeId)
    {
        if (managerEmployeeId == Id)
        {
            throw new InvalidOperationException("Employee cannot manage itself.");
        }
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static string? NormalizeEmail(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToLowerInvariant();
    }
}
