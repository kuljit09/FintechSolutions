using Banking.Application.DTOs;

namespace Banking.Application.Interfaces.Services;

public interface ILoanService
{
    Task<LoanDto?> GetLoanStatusAsync(Guid customerId, Guid loanId);
    Task<IReadOnlyList<LoanRepaymentDto>> GetRepaymentScheduleAsync(Guid loanId);

    /// <summary>LOW-RISK WRITE - creates a loan application record using LoanEligibilityPolicy.</summary>
    Task<LoanDto> ApplyAsync(LoanApplicationRequest request);
}
