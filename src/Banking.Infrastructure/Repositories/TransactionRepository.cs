using Banking.Application.Interfaces.Repositories;
using Banking.Domain.Entities;
using Banking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Repositories;

public class TransactionRepository(AppDbContext db) : ITransactionRepository
{
    public Task<Transaction?> GetByIdAsync(Guid transactionId) =>
        db.Transactions.Include(t => t.Account).FirstOrDefaultAsync(t => t.Id == transactionId);

    public async Task<IReadOnlyList<Transaction>> GetByAccountAsync(Guid accountId, int take = 15) =>
        await db.Transactions.Where(t => t.AccountId == accountId)
                              .OrderByDescending(t => t.Timestamp)
                              .Take(take)
                              .ToListAsync();

    public Task<int> CountByAccountSinceAsync(Guid accountId, DateTime since) =>
        db.Transactions.CountAsync(t => t.AccountId == accountId && t.Timestamp >= since);

    /// <summary>Used by the fraud-sweep BackgroundService - see Banking.Infrastructure.BackgroundServices.</summary>
    public async Task<IReadOnlyList<Transaction>> GetUnsweptForFraudAsync(int take = 200) =>
        await db.Transactions.Where(t => !t.FlaggedForFraudReview)
                              .OrderBy(t => t.Timestamp)
                              .Take(take)
                              .ToListAsync();

    public async Task MarkSweptAsync(Guid transactionId)
    {
        var t = await db.Transactions.FirstOrDefaultAsync(x => x.Id == transactionId);
        if (t is null) return;
        t.FlaggedForFraudReview = true;
        await db.SaveChangesAsync();
    }
}
