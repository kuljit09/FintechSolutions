namespace Banking.Domain.Rules;

/// <summary>
/// Deliberately simplified credit decision rule for a learning project - real underwriting
/// involves far more (income verification, DTI ratio, bureau checks, collateral). The point
/// here is the PATTERN: a transparent, explainable domain rule the chatbot can cite, rather
/// than an LLM inventing a decision.
/// </summary>
public static class LoanEligibilityPolicy
{
    private const int MinimumCreditScore = 650;
    private const decimal MaxLoanToIncomeMultiplier = 5m; // illustrative only

    public static (bool Eligible, string Reason) Evaluate(int creditScore, decimal requestedAmount, decimal customerAnnualIncomeEstimate)
    {
        if (creditScore < MinimumCreditScore)
            return (false, $"Credit score {creditScore} is below the minimum required score of {MinimumCreditScore}.");

        var maxEligibleAmount = customerAnnualIncomeEstimate * MaxLoanToIncomeMultiplier;
        if (requestedAmount > maxEligibleAmount)
            return (false, $"Requested amount {requestedAmount:C} exceeds the maximum eligible amount of {maxEligibleAmount:C} based on income.");

        return (true, "Meets credit score and income-based eligibility criteria.");
    }
}
