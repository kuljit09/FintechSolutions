using Banking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Beneficiary> Beneficiaries => Set<Beneficiary>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<LoanRepayment> LoanRepayments => Set<LoanRepayment>();
    public DbSet<Dispute> Disputes => Set<Dispute>();
    public DbSet<FraudAlert> FraudAlerts => Set<FraudAlert>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    public DbSet<KnowledgeBaseArticle> KnowledgeBaseArticles => Set<KnowledgeBaseArticle>();
    public DbSet<KnowledgeBaseEmbedding> KnowledgeBaseEmbeddings => Set<KnowledgeBaseEmbedding>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // You already have pgvector installed locally - this just declares the extension to EF,
        // it does not attempt to CREATE it (EF's migration will emit CREATE EXTENSION IF NOT EXISTS).
        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<KnowledgeBaseEmbedding>(e =>
        {
            e.ToTable("knowledge_base_embeddings");
            e.HasKey(x => x.Id);
            e.Property(x => x.Embedding).HasColumnType("vector(768)");
        });

        // Enum-to-string conversions, applied centrally so every entity configuration stays terse.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.ClrType.GetProperties())
            {
                if (property.PropertyType.IsEnum)
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(property.Name)
                        .HasConversion<string>()
                        .HasMaxLength(30);
                }
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}
