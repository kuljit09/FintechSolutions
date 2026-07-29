using System.ComponentModel;
using System.Text.Json;
using Banking.AI.McpClient;
using Banking.AI.Memory;
using Microsoft.SemanticKernel;

namespace Banking.AI.Plugins;

public class CardPlugin(IMcpToolClient mcp, PendingConfirmationStore pendingStore, ConversationContext conversationContext)
{
    [KernelFunction("get_card_status")]
    [Description("Gets a card's current status (active, blocked, expired).")]
    public async Task<string> GetCardStatus([Description("The card's unique id")] Guid cardId)
    {
        var result = await mcp.CallToolAsync<object>("GetCardStatus", new { cardId });
        return JsonSerializer.Serialize(result);
    }

    [KernelFunction("request_block_card")]
    [Description("""
        HIGH-RISK SECURITY ACTION. Call this when the customer asks to block/freeze a lost or
        stolen card. This does NOT block the card immediately - it only stages the request and
        asks the customer to explicitly confirm in their next message. The actual block is
        executed by the host application after confirmation is detected, never directly by you.
        """)]
    public Task<string> RequestBlockCard(
        [Description("The card's unique id")] Guid cardId,
        [Description("Why the card should be blocked, e.g. 'reported lost'")] string reason)
    {
        var conversationId = conversationContext.ConversationId
            ?? throw new InvalidOperationException("No active conversation context - ChatOrchestrator must set ConversationContext.ConversationId before invoking the kernel.");

        pendingStore.Stage(conversationId, "BlockCard", new { cardId, reason });

        // This return value becomes part of the grounded prompt's tool-results context, so the
        // model's final answer naturally asks the customer to confirm rather than claiming done.
        return Task.FromResult($"Blocking card {cardId} is a security-sensitive action and has NOT been performed yet. " +
                                "Ask the customer to explicitly confirm (e.g. 'yes, block it') before anything happens.");
    }
}
