using Banking.Application.Interfaces.Repositories;
using Banking.Domain.Entities;
using Banking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Repositories;

public class FraudAlertRepository(AppDbContext db) : IFraudAlertRepository
{
    public async Task<IReadOnlyList<FraudAlert>> GetByAccountAsync(Guid accountId) =>
        await db.FraudAlerts.Where(a => a.AccountId == accountId).OrderByDescending(a => a.DetectedAt).ToListAsync();

    public async Task<FraudAlert> AddAsync(FraudAlert alert)
    {
        db.FraudAlerts.Add(alert);
        await db.SaveChangesAsync();
        return alert;
    }
}
