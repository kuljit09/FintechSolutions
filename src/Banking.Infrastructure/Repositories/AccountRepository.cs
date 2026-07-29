using Banking.Application.Interfaces.Repositories;
using Banking.Domain.Entities;
using Banking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Repositories;

public class AccountRepository(AppDbContext db) : IAccountRepository
{
    public Task<Account?> GetByIdAsync(Guid accountId) => db.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

    public Task<Account?> GetByIdForCustomerAsync(Guid customerId, Guid accountId) =>
        db.Accounts.FirstOrDefaultAsync(a => a.Id == accountId && a.CustomerId == customerId);

    public async Task<IReadOnlyList<Account>> GetByCustomerAsync(Guid customerId) =>
        await db.Accounts.Where(a => a.CustomerId == customerId).ToListAsync();
}
