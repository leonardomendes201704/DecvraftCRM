using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("ApplicationUsers");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Name).HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.Email).HasMaxLength(256).IsRequired();
        builder.Property(entity => entity.PasswordHash).HasMaxLength(500).IsRequired();

        builder.HasIndex(entity => new { entity.TenantId, entity.Email }).IsUnique();

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(entity => entity.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
