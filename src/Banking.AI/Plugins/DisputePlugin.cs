using System.ComponentModel;
using System.Text.Json;
using Banking.AI.McpClient;
using Microsoft.SemanticKernel;

namespace Banking.AI.Plugins;

public class DisputePlugin(IMcpToolClient mcp)
{
    [KernelFunction("check_dispute_eligibility")]
    [Description("Checks whether a transaction is eligible to be disputed, and explains why or why not.")]
    public async Task<string> CheckDisputeEligibility([Description("The transaction's unique id")] Guid transactionId)
    {
        var result = await mcp.CallToolAsync<object>("CheckDisputeEligibility", new { transactionId });
        return JsonSerializer.Serialize(result);
    }

    [KernelFunction("file_dispute")]
    [Description("""
        LOW-RISK WRITE: files a dispute for an eligible transaction. Unlike blocking a card, this
        is reversible and non-monetary at filing time (it starts an investigation, it does not
        move money), so it executes immediately without a separate confirmation turn - still
        always re-validate eligibility first via check_dispute_eligibility.
        """)]
    public async Task<string> FileDispute(
        [Description("The transaction's unique id")] Guid transactionId,
        [Description("The customer's stated reason for disputing this transaction")] string reason)
    {
        var result = await mcp.CallToolAsync<object>("FileDispute", new { transactionId, reason });
        return JsonSerializer.Serialize(result);
    }

    [KernelFunction("get_dispute_status")]
    [Description("Gets the current status and resolution notes for a filed dispute.")]
    public async Task<string> GetDisputeStatus([Description("The dispute's unique id")] Guid disputeId)
    {
        var result = await mcp.CallToolAsync<object>("GetDisputeStatus", new { disputeId });
        return JsonSerializer.Serialize(result);
    }
}
