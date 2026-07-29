using Banking.Domain.Enums;

namespace Banking.Domain.Entities;

public class Dispute
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TransactionId { get; set; }
    public Transaction? Transaction { get; set; }
    public DisputeStatus Status { get; set; } = DisputeStatus.Filed;
    public string Reason { get; set; } = default!;
    public DateTime FiledAt { get; set; } = DateTime.UtcNow;
    public string? ResolutionNotes { get; set; }
    public decimal? ResolvedAmount { get; set; }
}
