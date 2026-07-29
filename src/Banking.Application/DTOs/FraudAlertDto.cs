namespace Banking.Application.DTOs;

public record FraudAlertDto(Guid Id, Guid AccountId, Guid? TransactionId, string Severity, string Status, string Description, DateTime DetectedAt);
