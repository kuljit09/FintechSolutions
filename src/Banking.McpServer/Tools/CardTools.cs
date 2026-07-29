using System.ComponentModel;
using Banking.Application.DTOs;
using Banking.Application.Interfaces.Services;

namespace Banking.McpServer.Tools;

public class CardTools(ICardService cardService)
{
    /// <summary>ToolRiskTier.ReadOnly.</summary>
    [Description("Gets a card's current status (active, blocked, expired) and its masked number.")]
    public async Task<CardDto?> GetCardStatus(Guid cardId)
        => await cardService.GetCardStatusAsync(cardId);

    /// <summary>
    /// ToolRiskTier.HighRiskWrite. Exposed on the MCP surface for completeness/other agents,
    /// but Banking.AI's CardPlugin deliberately does NOT call this tool directly - it stages the
    /// request via PendingConfirmationStore and only Banking.AI's HighRiskActionExecutor (after
    /// explicit customer confirmation) triggers the real block. Document this asymmetry clearly
    /// for anyone else wiring an agent directly to this MCP server.
    /// </summary>
    [Description("Blocks a customer's card immediately. HIGH RISK - callers other than the vetted confirmation-gated flow in Banking.AI should not invoke this without their own equivalent safeguard.")]
    public async Task<BlockCardResultDto> BlockCard(Guid cardId, string reason)
        => await cardService.BlockCardAsync(cardId, reason);
}
