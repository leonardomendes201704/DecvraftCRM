using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class TenantBrandingConfiguration : IEntityTypeConfiguration<TenantBranding>
{
    public void Configure(EntityTypeBuilder<TenantBranding> builder)
    {
        builder.ToTable("TenantBrandings");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.SystemName).HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.LogoUrl).HasMaxLength(500);
        builder.Property(entity => entity.FaviconUrl).HasMaxLength(500);
        builder.Property(entity => entity.PrimaryColor).HasMaxLength(20).IsRequired();
        builder.Property(entity => entity.SecondaryColor).HasMaxLength(20).IsRequired();
        builder.Property(entity => entity.LoginBackgroundUrl).HasMaxLength(500);
        builder.Property(entity => entity.EmailSenderName).HasMaxLength(150);

        builder.HasIndex(entity => entity.TenantId).IsUnique();

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(entity => entity.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
