using Banking.Domain.Enums;

namespace Banking.Domain.Entities;

public class Card
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AccountId { get; set; }
    public Account? Account { get; set; }
    public string MaskedNumber { get; set; } = default!; // e.g. **** **** **** 4471
    public CardType Type { get; set; }
    public CardStatus Status { get; set; } = CardStatus.Active;
    public DateTime ExpiryDate { get; set; }
    public decimal DailyLimit { get; set; }
    public string? BlockReason { get; set; }
}
