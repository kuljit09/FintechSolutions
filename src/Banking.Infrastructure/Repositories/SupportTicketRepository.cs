using Banking.Application.Interfaces.Repositories;
using Banking.Domain.Entities;
using Banking.Infrastructure.Persistence;

namespace Banking.Infrastructure.Repositories;

public class SupportTicketRepository(AppDbContext db) : ISupportTicketRepository
{
    public async Task<SupportTicket> AddAsync(SupportTicket ticket)
    {
        db.SupportTickets.Add(ticket);
        await db.SaveChangesAsync();
        return ticket;
    }
}
