namespace Banking.AI.Memory;

/// <summary>
/// Deliberately simple keyword match, NOT an LLM call - the confirmation gate for a
/// security-sensitive action should not itself depend on a second model inference that could
/// be prompt-injected. Good enough for a learning project; a production system might use a
/// small classifier or a strict yes/no UI button instead of free-text parsing at all.
/// </summary>
public static class ConfirmationDetector
{
    private static readonly string[] Affirmations =
        ["yes", "confirm", "confirmed", "go ahead", "please do", "do it", "block it", "yes please"];

    public static bool LooksLikeConfirmation(string message) =>
        Affirmations.Any(a => message.Trim().Equals(a, StringComparison.OrdinalIgnoreCase)
                            || message.Trim().Contains(a, StringComparison.OrdinalIgnoreCase));
}
