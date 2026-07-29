using System.ComponentModel;
using System.Text.Json;
using Banking.AI.McpClient;
using Microsoft.SemanticKernel;

namespace Banking.AI.Plugins;

public class LoanPlugin(IMcpToolClient mcp)
{
    [KernelFunction("get_loan_status")]
    [Description("Gets the status, terms, and (if rejected) the reason for a customer's loan application.")]
    public async Task<string> GetLoanStatus(
        [Description("The customer's unique id")] Guid customerId,
        [Description("The loan's unique id")] Guid loanId)
    {
        var result = await mcp.CallToolAsync<object>("GetLoanStatus", new { customerId, loanId });
        return JsonSerializer.Serialize(result);
    }

    [KernelFunction("get_loan_repayment_schedule")]
    [Description("Gets the repayment schedule (due dates and amounts) for an approved loan.")]
    public async Task<string> GetLoanRepaymentSchedule([Description("The loan's unique id")] Guid loanId)
    {
        var result = await mcp.CallToolAsync<object>("GetLoanRepaymentSchedule", new { loanId });
        return JsonSerializer.Serialize(result);
    }
}
