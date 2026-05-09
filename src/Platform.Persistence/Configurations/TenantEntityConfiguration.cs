using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class TenantEntityConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Name).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.Slug).HasMaxLength(120).IsRequired();
        builder.Property(entity => entity.CustomDomain).HasMaxLength(255);

        builder.HasIndex(entity => entity.Slug).IsUnique();
        builder.HasIndex(entity => entity.CustomDomain).IsUnique().HasFilter("[CustomDomain] IS NOT NULL");
    }
}
