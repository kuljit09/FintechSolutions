namespace Banking.Application.DTOs;

public record AccountDto(Guid Id, string AccountNumber, string Type, string Status, decimal Balance, decimal OverdraftLimit, string Currency);
