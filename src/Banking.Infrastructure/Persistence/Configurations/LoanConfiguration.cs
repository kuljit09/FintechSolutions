using Banking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Banking.Infrastructure.Persistence.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("loans");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.PrincipalAmount).HasColumnType("numeric(14,2)");
        builder.Property(l => l.InterestRatePercent).HasColumnType("numeric(5,2)");
        builder.HasOne(l => l.Customer).WithMany().HasForeignKey(l => l.CustomerId);
        builder.HasMany(l => l.RepaymentSchedule).WithOne(r => r.Loan!).HasForeignKey(r => r.LoanId);
    }
}
