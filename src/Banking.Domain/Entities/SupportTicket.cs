using Banking.Domain.Enums;

namespace Banking.Domain.Entities;

public class SupportTicket
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public Guid? AccountId { get; set; }
    public string Subject { get; set; } = default!;
    public string Description { get; set; } = default!;
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
