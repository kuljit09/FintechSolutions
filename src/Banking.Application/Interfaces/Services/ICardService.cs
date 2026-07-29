using Banking.Application.DTOs;

namespace Banking.Application.Interfaces.Services;

public interface ICardService
{
    Task<CardDto?> GetCardStatusAsync(Guid cardId);

    /// <summary>
    /// HIGH-RISK WRITE. Security-critical - the MCP tool wrapping this should be flagged
    /// ToolRiskTier.HighRiskWrite and the orchestrator should require an explicit "yes, block it"
    /// confirmation turn from the customer before invoking it (see Banking.AI ChatOrchestrator).
    /// </summary>
    Task<BlockCardResultDto> BlockCardAsync(Guid cardId, string reason);
}
