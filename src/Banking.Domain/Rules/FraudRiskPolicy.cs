using Banking.Domain.Entities;

namespace Banking.Domain.Rules;

/// <summary>
/// Simple, explainable heuristic (NOT a real ML fraud model) used by the background
/// fraud-sweep service. Demonstrates the "near real-time" event-driven piece of this project:
/// transactions are periodically re-evaluated and flagged, independent of any chat request.
/// </summary>
public static class FraudRiskPolicy
{
    private const decimal HighValueThreshold = 75000m;

    public static (bool IsSuspicious, string Reason, Enums.FraudAlertSeverity Severity) Evaluate(
        Transaction transaction, int transactionsInLastHourForAccount)
    {
        if (transaction.Amount >= HighValueThreshold && transactionsInLastHourForAccount == 1)
            return (true, $"Unusually high-value transaction ({transaction.Amount:C}) with no similar recent activity on this account.", Enums.FraudAlertSeverity.High);

        if (transactionsInLastHourForAccount >= 5)
            return (true, $"{transactionsInLastHourForAccount} transactions within the last hour - unusual velocity.", Enums.FraudAlertSeverity.Medium);

        if (transaction.Amount >= HighValueThreshold)
            return (true, $"High-value transaction ({transaction.Amount:C}) flagged for routine review.", Enums.FraudAlertSeverity.Low);

        return (false, "No anomaly detected.", Enums.FraudAlertSeverity.Low);
    }
}
