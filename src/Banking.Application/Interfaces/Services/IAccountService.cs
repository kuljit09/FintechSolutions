using Banking.Application.DTOs;

namespace Banking.Application.Interfaces.Services;

public interface IAccountService
{
    Task<AccountDto?> GetAccountBalanceAsync(Guid customerId, Guid accountId);
    Task<IReadOnlyList<AccountDto>> GetCustomerAccountsAsync(Guid customerId);
}
