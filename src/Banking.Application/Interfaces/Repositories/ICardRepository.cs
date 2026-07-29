using Banking.Domain.Entities;

namespace Banking.Application.Interfaces.Repositories;

public interface ICardRepository
{
    Task<Card?> GetByIdAsync(Guid cardId);
    Task UpdateAsync(Card card);
}
