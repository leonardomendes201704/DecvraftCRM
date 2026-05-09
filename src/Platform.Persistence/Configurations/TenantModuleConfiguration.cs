using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class TenantModuleConfiguration : IEntityTypeConfiguration<TenantModule>
{
    public void Configure(EntityTypeBuilder<TenantModule> builder)
    {
        builder.ToTable("TenantModules");
        builder.HasKey(entity => entity.Id);

        builder.HasIndex(entity => new { entity.TenantId, entity.ModuleId }).IsUnique();

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(entity => entity.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Module>()
            .WithMany()
            .HasForeignKey(entity => entity.ModuleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
