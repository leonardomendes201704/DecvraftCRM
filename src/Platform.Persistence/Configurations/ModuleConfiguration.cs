using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;
using DomainModule = Platform.Domain.Entities.Module;

namespace Platform.Persistence.Configurations;

public sealed class ModuleConfiguration : IEntityTypeConfiguration<DomainModule>
{
    public void Configure(EntityTypeBuilder<DomainModule> builder)
    {
        builder.ToTable("Modules");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Name).HasMaxLength(100).IsRequired();
        builder.Property(entity => entity.Slug).HasMaxLength(100).IsRequired();
        builder.Property(entity => entity.Version).HasMaxLength(50).IsRequired();

        builder.HasIndex(entity => entity.Slug).IsUnique();
    }
}
