using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Key).HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.Description).HasMaxLength(300).IsRequired();
        builder.Property(entity => entity.Module).HasMaxLength(100).IsRequired();

        builder.HasIndex(entity => entity.Key).IsUnique();
    }
}
