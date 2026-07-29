using Banking.Application.Interfaces.Repositories;
using Banking.Domain.Entities;
using Banking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Repositories;

public class CardRepository(AppDbContext db) : ICardRepository
{
    public Task<Card?> GetByIdAsync(Guid cardId) => db.Cards.FirstOrDefaultAsync(c => c.Id == cardId);

    public async Task UpdateAsync(Card card)
    {
        db.Cards.Update(card);
        await db.SaveChangesAsync();
    }
}
