using Banking.Application.Interfaces.Repositories;
using Banking.Domain.Entities;
using Banking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Repositories;

public class DisputeRepository(AppDbContext db) : IDisputeRepository
{
    public Task<bool> ExistsForTransactionAsync(Guid transactionId) =>
        db.Disputes.AnyAsync(d => d.TransactionId == transactionId);

    public async Task<Dispute> AddAsync(Dispute dispute)
    {
        db.Disputes.Add(dispute);
        await db.SaveChangesAsync();
        return dispute;
    }

    public Task<Dispute?> GetByIdAsync(Guid disputeId) => db.Disputes.FirstOrDefaultAsync(d => d.Id == disputeId);
}
