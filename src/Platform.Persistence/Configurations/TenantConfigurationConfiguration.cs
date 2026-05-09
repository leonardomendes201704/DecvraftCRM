using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;
using DomainTenantConfiguration = Platform.Domain.Entities.TenantConfiguration;

namespace Platform.Persistence.Configurations;

public sealed class TenantConfigurationConfiguration : IEntityTypeConfiguration<DomainTenantConfiguration>
{
    public void Configure(EntityTypeBuilder<DomainTenantConfiguration> builder)
    {
        builder.ToTable("TenantConfigurations");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Key).HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.Value).HasMaxLength(4000).IsRequired();

        builder.HasIndex(entity => new { entity.TenantId, entity.Key }).IsUnique();

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(entity => entity.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
