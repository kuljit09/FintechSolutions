using Banking.Domain.Entities;

namespace Banking.Application.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid transactionId);
    Task<IReadOnlyList<Transaction>> GetByAccountAsync(Guid accountId, int take = 15);
    Task<int> CountByAccountSinceAsync(Guid accountId, DateTime since);
    Task<IReadOnlyList<Transaction>> GetUnsweptForFraudAsync(int take = 200);
    Task MarkSweptAsync(Guid transactionId);
}
