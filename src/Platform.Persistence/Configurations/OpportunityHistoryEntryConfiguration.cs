using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class OpportunityHistoryEntryConfiguration : IEntityTypeConfiguration<OpportunityHistoryEntry>
{
    public void Configure(EntityTypeBuilder<OpportunityHistoryEntry> builder)
    {
        builder.ToTable("OpportunityHistoryEntries");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Type).HasConversion<int>().IsRequired();
        builder.Property(entity => entity.Description).HasMaxLength(500).IsRequired();

        builder.HasIndex(entity => new { entity.TenantId, entity.OpportunityId, entity.CreatedAt });

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(entity => entity.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Opportunity>()
            .WithMany()
            .HasForeignKey(entity => new { entity.TenantId, entity.OpportunityId })
            .HasPrincipalKey(entity => new { entity.TenantId, entity.Id })
            .OnDelete(DeleteBehavior.Restrict);
    }
}
