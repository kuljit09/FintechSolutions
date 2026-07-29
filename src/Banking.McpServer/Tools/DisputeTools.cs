using System.ComponentModel;
using Banking.Application.DTOs;
using Banking.Application.Interfaces.Services;

namespace Banking.McpServer.Tools;

public class DisputeTools(IDisputeService disputeService)
{
    /// <summary>ToolRiskTier.ReadOnly.</summary>
    [Description("Checks whether a transaction is eligible to be disputed, and why or why not.")]
    public async Task<DisputeEligibilityDto> CheckDisputeEligibility(Guid transactionId)
        => await disputeService.CheckEligibilityAsync(transactionId);

    /// <summary>ToolRiskTier.LowRiskWrite - reversible, non-monetary at filing time, executes immediately.</summary>
    [Description("Files a dispute for an eligible transaction, starting an investigation.")]
    public async Task<DisputeDto> FileDispute(Guid transactionId, string reason)
        => await disputeService.FileDisputeAsync(new FileDisputeRequest(transactionId, reason));

    /// <summary>ToolRiskTier.ReadOnly.</summary>
    [Description("Gets the current status and resolution notes for a filed dispute.")]
    public async Task<DisputeDto?> GetDisputeStatus(Guid disputeId)
        => await disputeService.GetStatusAsync(disputeId);
}
