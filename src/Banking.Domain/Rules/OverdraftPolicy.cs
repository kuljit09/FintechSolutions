using Banking.Domain.Entities;

namespace Banking.Domain.Rules;

/// <summary>
/// Pure domain rule used to explain WHY a debit transaction failed - the "why did my
/// payment fail" chatbot scenario is grounded in this, not in an LLM guess.
/// </summary>
public static class OverdraftPolicy
{
    public static (bool WouldSucceed, string Reason) Evaluate(Account account, decimal debitAmount)
    {
        var availableFunds = account.Balance + account.OverdraftLimit;

        if (account.Status != Enums.AccountStatus.Active)
            return (false, $"Account is {account.Status}, transactions are blocked.");

        if (debitAmount > availableFunds)
            return (false, $"Insufficient funds: balance {account.Balance:C} + overdraft limit {account.OverdraftLimit:C} " +
                           $"is less than the requested {debitAmount:C}.");

        return (true, "Sufficient funds available.");
    }
}
