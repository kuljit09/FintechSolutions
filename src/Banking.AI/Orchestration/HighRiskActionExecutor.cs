using System.Text.Json;
using Banking.Application.Interfaces.Services;

namespace Banking.AI.Orchestration;

/// <summary>
/// Executes a previously-staged HighRiskWrite action once the customer has confirmed it.
/// Deliberately calls Application-layer services directly (in-process), NOT the MCP tool and
/// NOT the LLM again - once confirmation is detected, the host application is the one
/// triggering the action, with a fixed, known set of arguments captured at staging time. This
/// removes the model from the actual trigger path for security-sensitive actions entirely.
/// </summary>
public class HighRiskActionExecutor(ICardService cardService)
{
    public async Task<string> ExecuteAsync(string toolName, string argsJson)
    {
        switch (toolName)
        {
            case "BlockCard":
            {
                var args = JsonSerializer.Deserialize<BlockCardArgs>(argsJson)
                    ?? throw new InvalidOperationException("Malformed staged BlockCard arguments.");
                var result = await cardService.BlockCardAsync(args.CardId, args.Reason);
                return result.Success
                    ? $"Confirmed - card has been blocked. {result.Message}"
                    : $"Could not block the card: {result.Message}";
            }
            default:
                throw new InvalidOperationException($"No executor registered for staged high-risk action '{toolName}'.");
        }
    }

    private record BlockCardArgs(Guid CardId, string Reason);
}
