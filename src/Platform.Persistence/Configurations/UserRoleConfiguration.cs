using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");
        builder.HasKey(entity => entity.Id);

        builder.HasIndex(entity => new { entity.TenantId, entity.UserId, entity.RoleId }).IsUnique();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(entity => entity.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(entity => entity.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
