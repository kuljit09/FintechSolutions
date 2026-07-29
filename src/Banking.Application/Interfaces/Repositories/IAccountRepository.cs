using Banking.Domain.Entities;

namespace Banking.Application.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid accountId);
    Task<Account?> GetByIdForCustomerAsync(Guid customerId, Guid accountId);
    Task<IReadOnlyList<Account>> GetByCustomerAsync(Guid customerId);
}
