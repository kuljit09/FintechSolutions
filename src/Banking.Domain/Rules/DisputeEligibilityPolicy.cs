using Banking.Domain.Entities;
using Banking.Domain.Enums;

namespace Banking.Domain.Rules;

public static class DisputeEligibilityPolicy
{
    private static readonly TimeSpan DisputeWindow = TimeSpan.FromDays(60);

    public static (bool Eligible, string Reason) Evaluate(Transaction transaction, bool alreadyDisputed)
    {
        if (alreadyDisputed)
            return (false, "A dispute has already been filed for this transaction.");

        if (transaction.Status != TransactionStatus.Completed)
            return (false, $"Only completed transactions can be disputed (this one is {transaction.Status}).");

        var age = DateTime.UtcNow - transaction.Timestamp;
        if (age > DisputeWindow)
            return (false, $"The {DisputeWindow.Days}-day dispute window for this transaction has expired.");

        return (true, "Eligible to file a dispute.");
    }
}
