using Banking.Application.Interfaces.Repositories;
using Banking.Domain.Entities;
using Banking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Repositories;

public class LoanRepository(AppDbContext db) : ILoanRepository
{
    public Task<Loan?> GetByIdAsync(Guid loanId) =>
        db.Loans.Include(l => l.RepaymentSchedule).FirstOrDefaultAsync(l => l.Id == loanId);

    public Task<Loan?> GetByIdForCustomerAsync(Guid customerId, Guid loanId) =>
        db.Loans.Include(l => l.RepaymentSchedule).FirstOrDefaultAsync(l => l.Id == loanId && l.CustomerId == customerId);

    public async Task<Loan> AddAsync(Loan loan)
    {
        db.Loans.Add(loan);
        await db.SaveChangesAsync();
        return loan;
    }

    public async Task<IReadOnlyList<LoanRepayment>> GetRepaymentScheduleAsync(Guid loanId) =>
        await db.LoanRepayments.Where(r => r.LoanId == loanId).OrderBy(r => r.DueDate).ToListAsync();
}
