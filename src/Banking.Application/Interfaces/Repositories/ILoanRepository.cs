using Banking.Domain.Entities;

namespace Banking.Application.Interfaces.Repositories;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(Guid loanId);
    Task<Loan?> GetByIdForCustomerAsync(Guid customerId, Guid loanId);
    Task<Loan> AddAsync(Loan loan);
    Task<IReadOnlyList<LoanRepayment>> GetRepaymentScheduleAsync(Guid loanId);
}
