using Banking.Domain.Entities;

namespace Banking.Application.Interfaces.Repositories;

public interface IFraudAlertRepository
{
    Task<IReadOnlyList<FraudAlert>> GetByAccountAsync(Guid accountId);
    Task<FraudAlert> AddAsync(FraudAlert alert);
}
