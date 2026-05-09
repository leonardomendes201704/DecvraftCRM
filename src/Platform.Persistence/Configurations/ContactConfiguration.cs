using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Name).HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.Email).HasMaxLength(256).IsRequired();
        builder.Property(entity => entity.Phone).HasMaxLength(40);
        builder.Property(entity => entity.Role).HasMaxLength(100);

        builder.HasIndex(entity => new { entity.TenantId, entity.CustomerId, entity.Email }).IsUnique();

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(entity => entity.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(entity => new { entity.TenantId, entity.CustomerId })
            .HasPrincipalKey(entity => new { entity.TenantId, entity.Id })
            .OnDelete(DeleteBehavior.Restrict);
    }
}
