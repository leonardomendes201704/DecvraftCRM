using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Entities;

namespace Platform.Persistence.Configurations;

public sealed class FinancialTransactionConfiguration : IEntityTypeConfiguration<FinancialTransaction>
{
    public void Configure(EntityTypeBuilder<FinancialTransaction> builder)
    {
        builder.ToTable("FinancialTransactions");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Description).HasMaxLength(250).IsRequired();
        builder.Property(entity => entity.Amount).HasPrecision(18, 2);
        builder.Property(entity => entity.Type).HasConversion<int>().IsRequired();
        builder.Property(entity => entity.Status).HasConversion<int>().IsRequired();

        builder.HasIndex(entity => new { entity.TenantId, entity.AccountId, entity.OccurredOn });

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(entity => entity.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<FinancialAccount>()
            .WithMany()
            .HasForeignKey(entity => new { entity.TenantId, entity.AccountId })
            .HasPrincipalKey(entity => new { entity.TenantId, entity.Id })
            .OnDelete(DeleteBehavior.Restrict);
    }
}
