using System.ComponentModel;
using Banking.Application.DTOs;
using Banking.Application.Interfaces.Services;

namespace Banking.McpServer.Tools;

/// <summary>ToolRiskTier.ReadOnly - all methods here are pure reads.</summary>
public class AccountTools(IAccountService accountService)
{
    [Description("Gets the current balance, overdraft limit, and status for a customer's account. Always pass the authenticated customer's id.")]
    public async Task<AccountDto?> GetAccountBalance(Guid customerId, Guid accountId)
        => await accountService.GetAccountBalanceAsync(customerId, accountId);

    [Description("Lists all accounts belonging to a customer.")]
    public async Task<IReadOnlyList<AccountDto>> GetCustomerAccounts(Guid customerId)
        => await accountService.GetCustomerAccountsAsync(customerId);
}
