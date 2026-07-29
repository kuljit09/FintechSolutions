namespace Banking.AI.Memory;

/// <summary>
/// Carries the current conversationId across the async call chain into Semantic Kernel plugin
/// methods, WITHOUT the LLM ever supplying or seeing it as a function parameter. Same principle
/// as never trusting an LLM-supplied customerId: identity/session context must come from the
/// host application, not from model output. Registered as a singleton - AsyncLocal gives correct
/// per-request isolation even though the service instance itself is shared.
/// </summary>
public class ConversationContext
{
    private static readonly AsyncLocal<string?> _current = new();

    public string? ConversationId
    {
        get => _current.Value;
        set => _current.Value = value;
    }
}
