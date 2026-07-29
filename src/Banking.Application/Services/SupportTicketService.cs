using Banking.Application.DTOs;
using Banking.Application.Interfaces.Repositories;
using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities;

namespace Banking.Application.Services;

public class SupportTicketService(ISupportTicketRepository tickets) : ISupportTicketService
{
    public async Task<SupportTicketDto> CreateAsync(CreateSupportTicketRequest request)
    {
        var ticket = new SupportTicket
        {
            CustomerId = request.CustomerId,
            AccountId = request.AccountId,
            Subject = request.Subject,
            Description = request.Description
        };
        var saved = await tickets.AddAsync(ticket);
        return new SupportTicketDto(saved.Id, saved.Subject, saved.Description, saved.Status.ToString(), saved.CreatedAt);
    }
}
