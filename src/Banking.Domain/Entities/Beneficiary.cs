namespace Banking.Domain.Entities;

public class Beneficiary
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AccountId { get; set; }
    public string Name { get; set; } = default!;
    public string BeneficiaryAccountNumber { get; set; } = default!;
    public string BankName { get; set; } = default!;
    public string? Nickname { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
