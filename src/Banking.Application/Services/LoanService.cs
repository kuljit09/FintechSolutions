using Banking.Application.DTOs;
using Banking.Application.Interfaces.Repositories;
using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities;
using Banking.Domain.Enums;
using Banking.Domain.Rules;

namespace Banking.Application.Services;

public class LoanService(ILoanRepository loans, ICustomerRepository customers) : ILoanService
{
    public async Task<LoanDto?> GetLoanStatusAsync(Guid customerId, Guid loanId)
    {
        var loan = await loans.GetByIdForCustomerAsync(customerId, loanId);
        return loan is null ? null : Map(loan);
    }

    public async Task<IReadOnlyList<LoanRepaymentDto>> GetRepaymentScheduleAsync(Guid loanId)
        => (await loans.GetRepaymentScheduleAsync(loanId))
            .Select(r => new LoanRepaymentDto(r.Id, r.DueDate, r.AmountDue, r.Status.ToString()))
            .ToList();

    public async Task<LoanDto> ApplyAsync(LoanApplicationRequest request)
    {
        var customer = await customers.GetByIdAsync(request.CustomerId)
            ?? throw new InvalidOperationException("Customer not found.");

        var (eligible, reason) = LoanEligibilityPolicy.Evaluate(
            customer.CreditScore, request.PrincipalAmount, request.AnnualIncomeEstimate);

        var loan = new Loan
        {
            CustomerId = request.CustomerId,
            Type = Enum.Parse<LoanType>(request.LoanType, ignoreCase: true),
            PrincipalAmount = request.PrincipalAmount,
            TermMonths = request.TermMonths,
            InterestRatePercent = eligible ? 11.5m : 0m, // illustrative flat rate
            CreditScoreAtApplication = customer.CreditScore,
            Status = eligible ? LoanStatus.Approved : LoanStatus.Rejected,
            RejectionReason = eligible ? null : reason
        };

        var saved = await loans.AddAsync(loan);
        return Map(saved);
    }

    private static LoanDto Map(Loan l) => new(
        l.Id, l.Type.ToString(), l.PrincipalAmount, l.InterestRatePercent, l.TermMonths,
        l.Status.ToString(), l.AppliedAt, l.RejectionReason);
}
