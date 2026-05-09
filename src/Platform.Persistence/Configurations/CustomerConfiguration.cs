using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(entity => entity.Id);
        builder.HasAlternateKey(entity => new { entity.TenantId, entity.Id });

        builder.Property(entity => entity.Name).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.Document).HasMaxLength(40).IsRequired();
        builder.Property(entity => entity.Type).HasConversion<int>().IsRequired();
        builder.Property(entity => entity.Status).HasConversion<int>().IsRequired();

        builder.HasIndex(entity => new { entity.TenantId, entity.Document }).IsUnique();
        builder.HasIndex(entity => new { entity.TenantId, entity.Name });

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(entity => entity.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
