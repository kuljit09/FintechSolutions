using System.ComponentModel;
using Banking.Application.DTOs;
using Banking.Application.Interfaces.Services;

namespace Banking.McpServer.Tools;

/// <summary>ToolRiskTier.ReadOnly.</summary>
public class TransactionTools(ITransactionService transactionService)
{
    [Description("Lists an account's most recent transactions, newest first.")]
    public async Task<IReadOnlyList<TransactionDto>> GetRecentTransactions(Guid accountId)
        => await transactionService.GetRecentTransactionsAsync(accountId);

    [Description("Explains why a specific transaction failed, grounded in the actual overdraft/balance rule evaluation.")]
    public async Task<TransactionFailureExplanationDto> ExplainTransactionFailure(Guid transactionId)
        => await transactionService.ExplainFailureAsync(transactionId);
}
