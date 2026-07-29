namespace Banking.Application.DTOs;

public record DisputeEligibilityDto(Guid TransactionId, bool Eligible, string Reason);

public record DisputeDto(Guid Id, Guid TransactionId, string Status, string Reason, DateTime FiledAt, string? ResolutionNotes, decimal? ResolvedAmount);

public record FileDisputeRequest(Guid TransactionId, string Reason);
