using System.ComponentModel;
using System.Text.Json;
using Banking.AI.McpClient;
using Microsoft.SemanticKernel;

namespace Banking.AI.Plugins;

public class SupportTicketPlugin(IMcpToolClient mcp)
{
    [KernelFunction("create_support_ticket")]
    [Description("LOW-RISK WRITE: creates a support ticket when the chatbot cannot resolve the issue itself.")]
    public async Task<string> CreateSupportTicket(
        [Description("The customer's unique id")] Guid customerId,
        [Description("Related account id, if any")] Guid? accountId,
        [Description("Short subject line")] string subject,
        [Description("Full description of the issue")] string description)
    {
        var result = await mcp.CallToolAsync<object>("CreateSupportTicket", new { customerId, accountId, subject, description });
        return JsonSerializer.Serialize(result);
    }
}
