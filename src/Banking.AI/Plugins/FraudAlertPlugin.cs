using System.ComponentModel;
using System.Text.Json;
using Banking.AI.McpClient;
using Microsoft.SemanticKernel;

namespace Banking.AI.Plugins;

public class FraudAlertPlugin(IMcpToolClient mcp)
{
    [KernelFunction("get_fraud_alerts")]
    [Description("""
        Gets fraud alerts already raised for an account by the background fraud-detection sweep.
        This tool only READS alert state - it never runs fraud detection itself; detection is a
        continuous background process independent of the chat request.
        """)]
    public async Task<string> GetFraudAlerts([Description("The account's unique id")] Guid accountId)
    {
        var result = await mcp.CallToolAsync<object>("GetFraudAlerts", new { accountId });
        return JsonSerializer.Serialize(result);
    }
}
