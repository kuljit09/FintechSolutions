using System.Collections.Concurrent;
using System.Text.Json;

namespace Banking.AI.Memory;

/// <summary>
/// THE key new concept this banking project adds over the e-commerce one: a human-in-the-loop
/// confirmation gate for HighRiskWrite tools (see Banking.Shared.Contracts.ToolRiskTier).
///
/// Pattern: when Semantic Kernel's function-calling loop decides to invoke a high-risk action
/// (e.g. block_card), the plugin method does NOT perform the action. It "stages" it here and
/// asks the customer to explicitly confirm. Only once the ChatOrchestrator sees a confirming
/// reply in the NEXT turn does the actual Application-layer call happen - and that execution is
/// done directly by orchestrator code, not by asking the LLM to "try calling the tool again".
/// This avoids ever trusting the model's own judgement for the actual trigger of a sensitive action.
/// </summary>
public class PendingConfirmationStore
{
    public record StagedAction(string ToolName, string ArgsJson, DateTime StagedAt);

    private readonly ConcurrentDictionary<string, StagedAction> _staged = new();

    public void Stage(string conversationId, string toolName, object args) =>
        _staged[conversationId] = new StagedAction(toolName, JsonSerializer.Serialize(args), DateTime.UtcNow);

    public StagedAction? GetStaged(string conversationId) =>
        _staged.TryGetValue(conversationId, out var action) ? action : null;

    public void Clear(string conversationId) => _staged.TryRemove(conversationId, out _);
}
