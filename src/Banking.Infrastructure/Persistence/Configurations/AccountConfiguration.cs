using Banking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Banking.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");
        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.AccountNumber).IsUnique();
        builder.Property(a => a.Balance).HasColumnType("numeric(14,2)");
        builder.Property(a => a.OverdraftLimit).HasColumnType("numeric(14,2)");
        builder.HasOne(a => a.Customer).WithMany(c => c.Accounts).HasForeignKey(a => a.CustomerId);
        builder.HasMany(a => a.Transactions).WithOne(t => t.Account!).HasForeignKey(t => t.AccountId);
        builder.HasMany(a => a.Cards).WithOne(c => c.Account!).HasForeignKey(c => c.AccountId);
    }
}
