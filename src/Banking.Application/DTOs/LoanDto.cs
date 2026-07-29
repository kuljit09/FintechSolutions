namespace Banking.Application.DTOs;

public record LoanDto(
    Guid Id, string Type, decimal PrincipalAmount, decimal InterestRatePercent, int TermMonths,
    string Status, DateTime AppliedAt, string? RejectionReason);

public record LoanRepaymentDto(Guid Id, DateTime DueDate, decimal AmountDue, string Status);

public record LoanApplicationRequest(Guid CustomerId, string LoanType, decimal PrincipalAmount, int TermMonths, decimal AnnualIncomeEstimate);

public record LoanEligibilityDto(bool Eligible, string Reason, int CreditScoreUsed);
