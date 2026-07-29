using Banking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Banking.Infrastructure.Persistence.Configurations;

public class DisputeConfiguration : IEntityTypeConfiguration<Dispute>
{
    public void Configure(EntityTypeBuilder<Dispute> builder)
    {
        builder.ToTable("disputes");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.ResolvedAmount).HasColumnType("numeric(14,2)");
        builder.HasOne(d => d.Transaction).WithMany().HasForeignKey(d => d.TransactionId);
    }
}
