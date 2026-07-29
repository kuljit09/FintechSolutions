using Banking.Application.DTOs;

namespace Banking.Application.Interfaces.Services;

public interface IDisputeService
{
    Task<DisputeEligibilityDto> CheckEligibilityAsync(Guid transactionId);

    /// <summary>LOW-RISK WRITE - reversible, non-monetary at filing time.</summary>
    Task<DisputeDto> FileDisputeAsync(FileDisputeRequest request);
    Task<DisputeDto?> GetStatusAsync(Guid disputeId);
}
