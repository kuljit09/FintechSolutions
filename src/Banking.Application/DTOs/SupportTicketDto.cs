namespace Banking.Application.DTOs;

public record SupportTicketDto(Guid Id, string Subject, string Description, string Status, DateTime CreatedAt);

public record CreateSupportTicketRequest(Guid CustomerId, Guid? AccountId, string Subject, string Description);
