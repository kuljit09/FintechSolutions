using Banking.Application.DTOs;

namespace Banking.Application.Interfaces.Services;

public interface IFraudAlertService
{
    Task<IReadOnlyList<FraudAlertDto>> GetAlertsForAccountAsync(Guid accountId);
}
