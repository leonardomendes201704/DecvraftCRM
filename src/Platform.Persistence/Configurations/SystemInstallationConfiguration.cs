using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class SystemInstallationConfiguration : IEntityTypeConfiguration<SystemInstallation>
{
    public void Configure(EntityTypeBuilder<SystemInstallation> builder)
    {
        builder.ToTable("SystemInstallations");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.InstallationKey).HasMaxLength(100).IsRequired();
        builder.Property(entity => entity.InstalledVersion).HasMaxLength(50).IsRequired();
        builder.Property(entity => entity.InstalledBy).HasMaxLength(256);
        builder.Property(entity => entity.Environment).HasMaxLength(50).IsRequired();

        builder.HasIndex(entity => entity.InstallationKey).IsUnique();
    }
}
