using System.ComponentModel;
using System.Text.Json;
using Banking.AI.McpClient;
using Microsoft.SemanticKernel;

namespace Banking.AI.Plugins;

public class TransactionPlugin(IMcpToolClient mcp)
{
    [KernelFunction("get_recent_transactions")]
    [Description("Lists a customer account's most recent transactions.")]
    public async Task<string> GetRecentTransactions([Description("The account's unique id")] Guid accountId)
    {
        var result = await mcp.CallToolAsync<object>("GetRecentTransactions", new { accountId });
        return JsonSerializer.Serialize(result);
    }

    [KernelFunction("explain_transaction_failure")]
    [Description("Explains WHY a specific transaction failed, grounded in the account's actual balance/overdraft rule evaluation rather than a guess.")]
    public async Task<string> ExplainTransactionFailure([Description("The transaction's unique id")] Guid transactionId)
    {
        var result = await mcp.CallToolAsync<object>("ExplainTransactionFailure", new { transactionId });
        return JsonSerializer.Serialize(result);
    }
}
