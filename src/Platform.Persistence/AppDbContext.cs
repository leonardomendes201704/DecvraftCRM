using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Platform.Application.Abstractions;
using Platform.Domain.Common;
using Platform.Domain.Entities;
using DomainModule = Platform.Domain.Entities.Module;

namespace Platform.Persistence;

public sealed class AppDbContext : DbContext
{
    private readonly Guid? _currentTenantId;

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentTenant? currentTenant = null)
        : base(options)
    {
        _currentTenantId = currentTenant?.TenantId;
    }

    public DbSet<SystemInstallation> SystemInstallations => Set<SystemInstallation>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<TenantBranding> TenantBrandings => Set<TenantBranding>();
    public DbSet<DomainModule> Modules => Set<DomainModule>();
    public DbSet<TenantModule> TenantModules => Set<TenantModule>();
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<SystemConfiguration> SystemConfigurations => Set<SystemConfiguration>();
    public DbSet<TenantConfiguration> TenantConfigurations => Set<TenantConfiguration>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Opportunity> Opportunities => Set<Opportunity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        ApplyTenantFilters(modelBuilder);
    }

    private void ApplyTenantFilters(ModelBuilder modelBuilder)
    {
        var method = typeof(AppDbContext)
            .GetMethod(nameof(SetTenantQueryFilter), BindingFlags.Instance | BindingFlags.NonPublic);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            method?.MakeGenericMethod(entityType.ClrType).Invoke(this, [modelBuilder]);
        }
    }

    private void SetTenantQueryFilter<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, ITenantEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(entity =>
            _currentTenantId == null || entity.TenantId == _currentTenantId);
    }
}
