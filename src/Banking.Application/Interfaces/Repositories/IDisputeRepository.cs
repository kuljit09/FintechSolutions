using Banking.Domain.Entities;

namespace Banking.Application.Interfaces.Repositories;

public interface IDisputeRepository
{
    Task<bool> ExistsForTransactionAsync(Guid transactionId);
    Task<Dispute> AddAsync(Dispute dispute);
    Task<Dispute?> GetByIdAsync(Guid disputeId);
}
