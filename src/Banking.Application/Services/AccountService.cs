using Banking.Application.DTOs;
using Banking.Application.Interfaces.Repositories;
using Banking.Application.Interfaces.Services;

namespace Banking.Application.Services;

/// <summary>
/// Reused identically by AccountsController and the MCP GetAccountBalance/GetCustomerAccounts tools -
/// same "one service layer, many consumers" pattern as the e-commerce project's OrderService.
/// </summary>
public class AccountService(IAccountRepository accounts) : IAccountService
{
    public async Task<AccountDto?> GetAccountBalanceAsync(Guid customerId, Guid accountId)
    {
        var a = await accounts.GetByIdForCustomerAsync(customerId, accountId);
        return a is null ? null : Map(a);
    }

    public async Task<IReadOnlyList<AccountDto>> GetCustomerAccountsAsync(Guid customerId)
        => (await accounts.GetByCustomerAsync(customerId)).Select(Map).ToList();

    private static AccountDto Map(Domain.Entities.Account a) =>
        new(a.Id, a.AccountNumber, a.Type.ToString(), a.Status.ToString(), a.Balance, a.OverdraftLimit, a.Currency);
}
