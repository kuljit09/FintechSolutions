using Banking.Domain.Enums;

namespace Banking.Domain.Entities;

public class Account
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public string AccountNumber { get; set; } = default!;
    public AccountType Type { get; set; }
    public AccountStatus Status { get; set; } = AccountStatus.Active;
    public decimal Balance { get; set; }
    public decimal OverdraftLimit { get; set; }
    public string Currency { get; set; } = "INR";

    public List<Transaction> Transactions { get; set; } = new();
    public List<Card> Cards { get; set; } = new();
}
