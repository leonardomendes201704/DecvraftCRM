using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(entity => entity.Id);
        builder.HasAlternateKey(entity => new { entity.TenantId, entity.Id });

        builder.Property(entity => entity.FullName).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.Document).HasMaxLength(40);
        builder.Property(entity => entity.CorporateEmail).HasMaxLength(256);
        builder.Property(entity => entity.Phone).HasMaxLength(40);
        builder.Property(entity => entity.Status).HasConversion<int>().IsRequired();

        builder.HasIndex(entity => new { entity.TenantId, entity.ApplicationUserId })
            .IsUnique()
            .HasFilter("[ApplicationUserId] IS NOT NULL");

        builder.HasIndex(entity => new { entity.TenantId, entity.CorporateEmail })
            .IsUnique()
            .HasFilter("[CorporateEmail] IS NOT NULL");

        builder.HasIndex(entity => new { entity.TenantId, entity.DepartmentId });
        builder.HasIndex(entity => new { entity.TenantId, entity.JobTitleId });
        builder.HasIndex(entity => new { entity.TenantId, entity.ManagerEmployeeId });
        builder.HasIndex(entity => new { entity.TenantId, entity.Status });

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(entity => entity.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<Employee>(entity => new { entity.TenantId, entity.ApplicationUserId })
            .HasPrincipalKey<ApplicationUser>(entity => new { entity.TenantId, entity.Id })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(entity => new { entity.TenantId, entity.DepartmentId })
            .HasPrincipalKey(entity => new { entity.TenantId, entity.Id })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<JobTitle>()
            .WithMany()
            .HasForeignKey(entity => new { entity.TenantId, entity.JobTitleId })
            .HasPrincipalKey(entity => new { entity.TenantId, entity.Id })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(entity => new { entity.TenantId, entity.ManagerEmployeeId })
            .HasPrincipalKey(entity => new { entity.TenantId, entity.Id })
            .OnDelete(DeleteBehavior.Restrict);
    }
}
