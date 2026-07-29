namespace Banking.Application.DTOs;

public record TransactionDto(
    Guid Id, Guid AccountId, string Type, string Status, decimal Amount, string Currency,
    string? Merchant, string? Description, string? FailureReason, DateTime Timestamp);

public record TransactionFailureExplanationDto(Guid TransactionId, bool Failed, string Explanation);
