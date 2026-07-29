using Banking.Domain.Enums;

namespace Banking.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public KycStatus KycStatus { get; set; } = KycStatus.Pending;
    public int CreditScore { get; set; }
    public List<Account> Accounts { get; set; } = new();
}
