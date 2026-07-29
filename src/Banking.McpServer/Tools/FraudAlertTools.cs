using System.ComponentModel;
using Banking.Application.DTOs;
using Banking.Application.Interfaces.Services;

namespace Banking.McpServer.Tools;

/// <summary>ToolRiskTier.ReadOnly - alerts are written only by the background fraud sweep, never by this tool.</summary>
public class FraudAlertTools(IFraudAlertService fraudAlertService)
{
    [Description("Gets fraud alerts already raised for an account by the background detection sweep.")]
    public async Task<IReadOnlyList<FraudAlertDto>> GetFraudAlerts(Guid accountId)
        => await fraudAlertService.GetAlertsForAccountAsync(accountId);
}
