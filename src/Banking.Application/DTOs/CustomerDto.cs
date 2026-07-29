namespace Banking.Application.DTOs;

public record CustomerDto(Guid Id, string FullName, string Email, string KycStatus, int CreditScore);
