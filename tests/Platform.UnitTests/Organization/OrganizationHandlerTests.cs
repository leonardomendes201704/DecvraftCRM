using Platform.Application.Abstractions;
using Platform.Application.Organization;
using Platform.Domain.Entities;
using Platform.Domain.Enums;

namespace Platform.UnitTests.Organization;

public sealed class OrganizationHandlerTests
{
    private static readonly DateTimeOffset Now = new(2026, 5, 10, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task CreateDepartment_returns_duplicate_code_when_code_exists()
    {
        var repository = new FakeDepartmentRepository { CodeExists = true };
        var handler = new CreateDepartmentCommandHandler(repository, new FixedClock(Now));

        var result = await handler.Handle(
            new CreateDepartmentCommand(Guid.NewGuid(), new CreateDepartmentRequest("Comercial", "SALES")),
            CancellationToken.None);

        Assert.Equal(OrganizationOperationStatus.DuplicateCode, result.Status);
        Assert.False(repository.Saved);
    }

    [Fact]
    public async Task CreateJobTitle_returns_invalid_input_when_level_is_not_positive()
    {
        var handler = new CreateJobTitleCommandHandler(new FakeJobTitleRepository(), new FixedClock(Now));

        var result = await handler.Handle(
            new CreateJobTitleCommand(Guid.NewGuid(), new CreateJobTitleRequest("Vendedor", "SALES_REP", 0, false)),
            CancellationToken.None);

        Assert.Equal(OrganizationOperationStatus.InvalidInput, result.Status);
    }

    [Fact]
    public async Task CreateEmployee_returns_related_entity_not_found_when_department_does_not_exist()
    {
        var repository = new FakeEmployeeRepository();
        var handler = new CreateEmployeeCommandHandler(repository, new FixedClock(Now));

        var result = await handler.Handle(
            new CreateEmployeeCommand(
                Guid.NewGuid(),
                new CreateEmployeeRequest(
                    Guid.NewGuid(),
                    null,
                    null,
                    "Maria Silva",
                    null,
                    "maria@acme.test",
                    null,
                    null)),
            CancellationToken.None);

        Assert.Equal(OrganizationOperationStatus.RelatedEntityNotFound, result.Status);
        Assert.False(repository.Saved);
    }

    [Fact]
    public async Task LinkEmployeeUser_returns_user_already_linked_when_user_has_another_employee()
    {
        var tenantId = Guid.NewGuid();
        var employee = Employee.Create(
            tenantId,
            null,
            null,
            null,
            null,
            "Ana Lima",
            null,
            null,
            null,
            null,
            Now);
        var repository = new FakeEmployeeRepository
        {
            Employee = employee,
            ApplicationUserExists = true,
            ApplicationUserLinked = true
        };
        var handler = new LinkEmployeeUserCommandHandler(repository, new FixedClock(Now));

        var result = await handler.Handle(
            new LinkEmployeeUserCommand(
                tenantId,
                employee.Id,
                new LinkEmployeeUserRequest(Guid.NewGuid())),
            CancellationToken.None);

        Assert.Equal(EmployeeLinkOperationStatus.UserAlreadyLinked, result.Status);
        Assert.False(repository.Saved);
    }

    private sealed class FixedClock : IClock
    {
        public FixedClock(DateTimeOffset utcNow)
        {
            UtcNow = utcNow;
        }

        public DateTimeOffset UtcNow { get; }
    }

    private sealed class FakeDepartmentRepository : IDepartmentRepository
    {
        public bool CodeExists { get; init; }
        public bool Saved { get; private set; }

        public Task<IReadOnlyCollection<Department>> ListAsync(Guid tenantId, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyCollection<Department>>([]);

        public Task<Department?> GetByIdAsync(Guid tenantId, Guid departmentId, CancellationToken cancellationToken = default) =>
            Task.FromResult<Department?>(null);

        public Task<bool> ExistsByCodeAsync(
            Guid tenantId,
            string code,
            Guid? exceptDepartmentId = null,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(CodeExists);

        public void Add(Department department)
        {
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            Saved = true;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeJobTitleRepository : IJobTitleRepository
    {
        public Task<IReadOnlyCollection<JobTitle>> ListAsync(Guid tenantId, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyCollection<JobTitle>>([]);

        public Task<JobTitle?> GetByIdAsync(Guid tenantId, Guid jobTitleId, CancellationToken cancellationToken = default) =>
            Task.FromResult<JobTitle?>(null);

        public Task<bool> ExistsByCodeAsync(
            Guid tenantId,
            string code,
            Guid? exceptJobTitleId = null,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public void Add(JobTitle jobTitle)
        {
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private sealed class FakeEmployeeRepository : IEmployeeRepository
    {
        public Employee? Employee { get; init; }
        public bool ApplicationUserExists { get; init; }
        public bool ApplicationUserLinked { get; init; }
        public bool Saved { get; private set; }

        public Task<IReadOnlyCollection<Employee>> ListAsync(Guid tenantId, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyCollection<Employee>>([]);

        public Task<Employee?> GetByIdAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken = default) =>
            Task.FromResult(Employee);

        public Task<Employee?> GetByApplicationUserIdAsync(
            Guid tenantId,
            Guid applicationUserId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<Employee?>(null);

        public Task<IReadOnlyCollection<Employee>> ListSubordinatesAsync(
            Guid tenantId,
            Guid managerEmployeeId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyCollection<Employee>>([]);

        public Task<bool> DepartmentExistsAsync(Guid tenantId, Guid departmentId, CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public Task<bool> JobTitleExistsAsync(Guid tenantId, Guid jobTitleId, CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public Task<bool> EmployeeExistsAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public Task<bool> ApplicationUserExistsAsync(
            Guid tenantId,
            Guid applicationUserId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(ApplicationUserExists);

        public Task<bool> ApplicationUserLinkedAsync(
            Guid tenantId,
            Guid applicationUserId,
            Guid? exceptEmployeeId = null,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(ApplicationUserLinked);

        public Task<bool> ExistsByCorporateEmailAsync(
            Guid tenantId,
            string corporateEmail,
            Guid? exceptEmployeeId = null,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public void Add(Employee employee)
        {
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            Saved = true;
            return Task.CompletedTask;
        }
    }
}
