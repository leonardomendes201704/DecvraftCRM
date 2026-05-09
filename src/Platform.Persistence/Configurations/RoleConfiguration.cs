using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Name).HasMaxLength(100).IsRequired();

        builder.HasIndex(entity => new { entity.TenantId, entity.Name }).IsUnique();

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(entity => entity.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
