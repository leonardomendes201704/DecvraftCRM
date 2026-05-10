using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.UnitTests.Organization;

public sealed class OrganizationDomainTests
{
    [Fact]
    public void Department_Create_sets_active_status_and_trimmed_values()
    {
        var department = Department.Create(Guid.NewGuid(), " Comercial ", " SALES ", DateTimeOffset.UtcNow);

        Assert.Equal("Comercial", department.Name);
        Assert.Equal("SALES", department.Code);
        Assert.True(department.IsActive);
    }

    [Fact]
    public void JobTitle_Create_requires_positive_level()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            JobTitle.Create(Guid.NewGuid(), "Vendedor", "SALES_REP", 0, false, DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Employee_Create_normalizes_optional_values_and_sets_active_status()
    {
        var employee = Employee.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            " Maria Silva ",
            " 12345678900 ",
            " MARIA@ACME.TEST ",
            " 11999990000 ",
            new DateOnly(2026, 1, 10),
            DateTimeOffset.UtcNow);

        Assert.Equal("Maria Silva", employee.FullName);
        Assert.Equal("12345678900", employee.Document);
        Assert.Equal("maria@acme.test", employee.CorporateEmail);
        Assert.Equal("11999990000", employee.Phone);
        Assert.Equal(EmployeeStatus.Active, employee.Status);
    }

    [Fact]
    public void Employee_Terminate_sets_termination_date_and_status()
    {
        var employee = Employee.Create(
            Guid.NewGuid(),
            null,
            null,
            null,
            null,
            "Joao Souza",
            null,
            null,
            null,
            null,
            DateTimeOffset.UtcNow);

        var terminationDate = new DateOnly(2026, 5, 10);
        employee.Terminate(terminationDate, DateTimeOffset.UtcNow);

        Assert.Equal(EmployeeStatus.Terminated, employee.Status);
        Assert.Equal(terminationDate, employee.TerminationDate);
    }

    [Fact]
    public void Employee_LinkUser_and_UnlinkUser_update_user_reference()
    {
        var employee = Employee.Create(
            Guid.NewGuid(),
            null,
            null,
            null,
            null,
            "Ana Lima",
            null,
            null,
            null,
            null,
            DateTimeOffset.UtcNow);

        var userId = Guid.NewGuid();
        employee.LinkUser(userId, DateTimeOffset.UtcNow);

        Assert.Equal(userId, employee.ApplicationUserId);

        employee.UnlinkUser(DateTimeOffset.UtcNow);

        Assert.Null(employee.ApplicationUserId);
    }

    [Fact]
    public void Organization_enums_have_stable_values()
    {
        Assert.Equal(1, (int)EmployeeStatus.Active);
        Assert.Equal(2, (int)EmployeeStatus.Inactive);
        Assert.Equal(3, (int)EmployeeStatus.OnLeave);
        Assert.Equal(4, (int)EmployeeStatus.Terminated);
    }
}
