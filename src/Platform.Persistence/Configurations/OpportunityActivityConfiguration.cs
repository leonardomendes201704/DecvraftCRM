using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class OpportunityActivityConfiguration : IEntityTypeConfiguration<OpportunityActivity>
{
    public void Configure(EntityTypeBuilder<OpportunityActivity> builder)
    {
        builder.ToTable("OpportunityActivities");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Type).HasConversion<int>().IsRequired();
        builder.Property(entity => entity.Title).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.Notes).HasMaxLength(2000);
        builder.Property(entity => entity.Status).HasConversion<int>().IsRequired();

        builder.HasIndex(entity => new { entity.TenantId, entity.OpportunityId, entity.Status });
        builder.HasIndex(entity => new { entity.TenantId, entity.OwnerEmployeeId });
        builder.HasIndex(entity => new { entity.TenantId, entity.DueAt });

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(entity => entity.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Opportunity>()
            .WithMany()
            .HasForeignKey(entity => new { entity.TenantId, entity.OpportunityId })
            .HasPrincipalKey(entity => new { entity.TenantId, entity.Id })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(entity => new { entity.TenantId, entity.OwnerEmployeeId })
            .HasPrincipalKey(entity => new { entity.TenantId, entity.Id })
            .OnDelete(DeleteBehavior.Restrict);
    }
}
