using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class SystemConfigurationConfiguration : IEntityTypeConfiguration<SystemConfiguration>
{
    public void Configure(EntityTypeBuilder<SystemConfiguration> builder)
    {
        builder.ToTable("SystemConfigurations");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Key).HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.Value).HasMaxLength(4000).IsRequired();

        builder.HasIndex(entity => entity.Key).IsUnique();
    }
}
