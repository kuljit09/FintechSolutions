using Banking.Application.DTOs;
using Banking.Application.Interfaces.Repositories;
using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities;
using Banking.Domain.Rules;

namespace Banking.Application.Services;

public class DisputeService(ITransactionRepository transactions, IDisputeRepository disputes) : IDisputeService
{
    public async Task<DisputeEligibilityDto> CheckEligibilityAsync(Guid transactionId)
    {
        var transaction = await transactions.GetByIdAsync(transactionId);
        if (transaction is null)
            return new DisputeEligibilityDto(transactionId, false, "Transaction not found.");

        var alreadyDisputed = await disputes.ExistsForTransactionAsync(transactionId);
        var (eligible, reason) = DisputeEligibilityPolicy.Evaluate(transaction, alreadyDisputed);
        return new DisputeEligibilityDto(transactionId, eligible, reason);
    }

    public async Task<DisputeDto> FileDisputeAsync(FileDisputeRequest request)
    {
        var eligibility = await CheckEligibilityAsync(request.TransactionId);
        if (!eligibility.Eligible)
            throw new InvalidOperationException($"Cannot file dispute: {eligibility.Reason}");

        var dispute = new Dispute { TransactionId = request.TransactionId, Reason = request.Reason };
        var saved = await disputes.AddAsync(dispute);
        return Map(saved);
    }

    public async Task<DisputeDto?> GetStatusAsync(Guid disputeId)
    {
        var d = await disputes.GetByIdAsync(disputeId);
        return d is null ? null : Map(d);
    }

    private static DisputeDto Map(Dispute d) =>
        new(d.Id, d.TransactionId, d.Status.ToString(), d.Reason, d.FiledAt, d.ResolutionNotes, d.ResolvedAmount);
}
