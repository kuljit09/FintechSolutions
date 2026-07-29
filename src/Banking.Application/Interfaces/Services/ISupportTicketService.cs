using Banking.Application.DTOs;

namespace Banking.Application.Interfaces.Services;

public interface ISupportTicketService
{
    Task<SupportTicketDto> CreateAsync(CreateSupportTicketRequest request);
}
