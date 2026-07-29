using Banking.Application.DTOs;
using Banking.Application.Interfaces.Repositories;
using Banking.Application.Interfaces.Services;

namespace Banking.Application.Services;

public class FraudAlertService(IFraudAlertRepository alerts) : IFraudAlertService
{
    public async Task<IReadOnlyList<FraudAlertDto>> GetAlertsForAccountAsync(Guid accountId)
        => (await alerts.GetByAccountAsync(accountId))
            .Select(a => new FraudAlertDto(a.Id, a.AccountId, a.TransactionId, a.Severity.ToString(), a.Status.ToString(), a.Description, a.DetectedAt))
            .ToList();
}
