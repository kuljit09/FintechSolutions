namespace Banking.Application.DTOs;

public record CardDto(Guid Id, Guid AccountId, string MaskedNumber, string Type, string Status, DateTime ExpiryDate, decimal DailyLimit, string? BlockReason);

public record BlockCardResultDto(Guid CardId, bool Success, string NewStatus, string Message);
