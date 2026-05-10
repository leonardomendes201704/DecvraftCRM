using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class JobTitleConfiguration : IEntityTypeConfiguration<JobTitle>
{
    public void Configure(EntityTypeBuilder<JobTitle> builder)
    {
        builder.ToTable("JobTitles");
        builder.HasKey(entity => entity.Id);
        builder.HasAlternateKey(entity => new { entity.TenantId, entity.Id });

        builder.Property(entity => entity.Name).HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.Code).HasMaxLength(60).IsRequired();
        builder.Property(entity => entity.Level).IsRequired();
        builder.Property(entity => entity.IsLeadership).IsRequired();
        builder.Property(entity => entity.IsActive).IsRequired();

        builder.HasIndex(entity => new { entity.TenantId, entity.Code }).IsUnique();
        builder.HasIndex(entity => new { entity.TenantId, entity.Level });

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(entity => entity.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
