using System.ComponentModel;
using Banking.Application.DTOs;
using Banking.Application.Interfaces.Services;

namespace Banking.McpServer.Tools;

/// <summary>ToolRiskTier.ReadOnly for status/schedule lookups.</summary>
public class LoanTools(ILoanService loanService)
{
    [Description("Gets the status, terms, and (if rejected) the reason for a customer's loan application.")]
    public async Task<LoanDto?> GetLoanStatus(Guid customerId, Guid loanId)
        => await loanService.GetLoanStatusAsync(customerId, loanId);

    [Description("Gets the repayment schedule for an approved/disbursed loan.")]
    public async Task<IReadOnlyList<LoanRepaymentDto>> GetLoanRepaymentSchedule(Guid loanId)
        => await loanService.GetRepaymentScheduleAsync(loanId);
}
