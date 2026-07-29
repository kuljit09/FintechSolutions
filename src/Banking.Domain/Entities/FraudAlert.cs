using Banking.Domain.Enums;

namespace Banking.Domain.Entities;

public class FraudAlert
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AccountId { get; set; }
    public Guid? TransactionId { get; set; }
    public FraudAlertSeverity Severity { get; set; }
    public FraudAlertStatus Status { get; set; } = FraudAlertStatus.Open;
    public string Description { get; set; } = default!;
    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
}
