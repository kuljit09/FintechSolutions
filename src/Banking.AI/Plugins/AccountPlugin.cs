using System.ComponentModel;
using System.Text.Json;
using Banking.AI.McpClient;
using Microsoft.SemanticKernel;

namespace Banking.AI.Plugins;

public class AccountPlugin(IMcpToolClient mcp)
{
    [KernelFunction("get_account_balance")]
    [Description("Gets the current balance, overdraft limit, and status for a customer's account.")]
    public async Task<string> GetAccountBalance(
        [Description("The customer's unique id")] Guid customerId,
        [Description("The account's unique id")] Guid accountId)
    {
        var result = await mcp.CallToolAsync<object>("GetAccountBalance", new { customerId, accountId });
        return JsonSerializer.Serialize(result);
    }

    [KernelFunction("get_customer_accounts")]
    [Description("Lists all accounts belonging to a customer.")]
    public async Task<string> GetCustomerAccounts([Description("The customer's unique id")] Guid customerId)
    {
        var result = await mcp.CallToolAsync<object>("GetCustomerAccounts", new { customerId });
        return JsonSerializer.Serialize(result);
    }
}
