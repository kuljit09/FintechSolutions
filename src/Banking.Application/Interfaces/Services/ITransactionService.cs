using Banking.Application.DTOs;

namespace Banking.Application.Interfaces.Services;

public interface ITransactionService
{
    Task<TransactionDto?> GetTransactionAsync(Guid accountId, Guid transactionId);
    Task<IReadOnlyList<TransactionDto>> GetRecentTransactionsAsync(Guid accountId);

    /// <summary>
    /// Grounds the "why did my transaction fail" chatbot scenario in the OverdraftPolicy
    /// domain rule rather than letting the LLM guess at a reason.
    /// </summary>
    Task<TransactionFailureExplanationDto> ExplainFailureAsync(Guid transactionId);
}
