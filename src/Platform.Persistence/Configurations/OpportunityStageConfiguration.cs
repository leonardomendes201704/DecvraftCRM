using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class OpportunityStageConfiguration : IEntityTypeConfiguration<OpportunityStage>
{
    public void Configure(EntityTypeBuilder<OpportunityStage> builder)
    {
        builder.ToTable("OpportunityStages");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Name).HasMaxLength(120).IsRequired();
        builder.Property(entity => entity.Position).IsRequired();
        builder.Property(entity => entity.IsActive).IsRequired();

        builder.HasIndex(entity => new { entity.TenantId, entity.Name }).IsUnique();
        builder.HasIndex(entity => new { entity.TenantId, entity.Position });

        builder.HasAlternateKey(entity => new { entity.TenantId, entity.Id });

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(entity => entity.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
