using Banking.Domain.Enums;

namespace Banking.Domain.Entities;

public class Loan
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public LoanType Type { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRatePercent { get; set; }
    public int TermMonths { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Applied;
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public int CreditScoreAtApplication { get; set; }
    public string? RejectionReason { get; set; }

    public List<LoanRepayment> RepaymentSchedule { get; set; } = new();
}
