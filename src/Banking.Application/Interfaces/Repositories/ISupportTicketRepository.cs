using Banking.Domain.Entities;

namespace Banking.Application.Interfaces.Repositories;

public interface ISupportTicketRepository
{
    Task<SupportTicket> AddAsync(SupportTicket ticket);
}
