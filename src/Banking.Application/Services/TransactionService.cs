using Banking.Application.DTOs;
using Banking.Application.Interfaces.Repositories;
using Banking.Application.Interfaces.Services;
using Banking.Domain.Enums;
using Banking.Domain.Rules;

namespace Banking.Application.Services;

public class TransactionService(ITransactionRepository transactions, IAccountRepository accounts) : ITransactionService
{
    public async Task<TransactionDto?> GetTransactionAsync(Guid accountId, Guid transactionId)
    {
        var t = await transactions.GetByIdAsync(transactionId);
        return t is null || t.AccountId != accountId ? null : Map(t);
    }

    public async Task<IReadOnlyList<TransactionDto>> GetRecentTransactionsAsync(Guid accountId)
        => (await transactions.GetByAccountAsync(accountId, Shared.Constants.AppConstants.MaxTransactionsReturnedToAgent))
            .Select(Map).ToList();

    public async Task<TransactionFailureExplanationDto> ExplainFailureAsync(Guid transactionId)
    {
        var t = await transactions.GetByIdAsync(transactionId);
        if (t is null)
            return new TransactionFailureExplanationDto(transactionId, false, "Transaction not found.");

        if (t.Status != TransactionStatus.Failed)
            return new TransactionFailureExplanationDto(transactionId, false, $"This transaction did not fail (status: {t.Status}).");

        var account = await accounts.GetByIdAsync(t.AccountId);
        if (account is null)
            return new TransactionFailureExplanationDto(transactionId, true, t.FailureReason ?? "Reason not recorded.");

        // Re-run the same OverdraftPolicy rule that (in a real system) the payment gateway
        // would have evaluated at authorization time, so the explanation is grounded in the
        // actual domain rule rather than just echoing a stored string.
        var (wouldSucceedNow, policyReason) = OverdraftPolicy.Evaluate(account, t.Amount);
        var explanation = t.FailureReason ?? policyReason;

        return new TransactionFailureExplanationDto(transactionId, true, explanation);
    }

    private static TransactionDto Map(Domain.Entities.Transaction t) => new(
        t.Id, t.AccountId, t.Type.ToString(), t.Status.ToString(), t.Amount, t.Currency,
        t.Merchant, t.Description, t.FailureReason, t.Timestamp);
}
