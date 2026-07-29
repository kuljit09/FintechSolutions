using Banking.Domain.Enums;

namespace Banking.Domain.Entities;

public class LoanRepayment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LoanId { get; set; }
    public Loan? Loan { get; set; }
    public DateTime DueDate { get; set; }
    public decimal AmountDue { get; set; }
    public RepaymentStatus Status { get; set; } = RepaymentStatus.Pending;
}
