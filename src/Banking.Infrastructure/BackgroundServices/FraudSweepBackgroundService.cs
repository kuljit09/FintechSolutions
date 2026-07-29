using Banking.Application.Interfaces.Repositories;
using Banking.Domain.Entities;
using Banking.Domain.Rules;
using Banking.Shared.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Banking.Infrastructure.BackgroundServices;

/// <summary>
/// THE "near real-time" piece of this project: independent of any chat request, this sweep
/// periodically re-evaluates new transactions against FraudRiskPolicy and raises FraudAlert
/// records. The chatbot's "why was there a fraud alert on my account" scenario is answered by
/// reading rows this service already wrote - it does not run fraud detection inline during chat.
///
/// This mirrors how real banking fraud systems work: detection is a continuous background
/// process (or a real-time event stream in production, e.g. Kafka + a rules/ML engine), and
/// the support channel (chat, call center, app) only ever *reads* alert state, never computes it.
/// </summary>
public class FraudSweepBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<FraudSweepBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Fraud sweep background service started - interval {Seconds}s", AppConstants.FraudSweep.IntervalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunSweepAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fraud sweep iteration failed - will retry next interval.");
            }

            await Task.Delay(TimeSpan.FromSeconds(AppConstants.FraudSweep.IntervalSeconds), stoppingToken);
        }
    }

    private async Task RunSweepAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var transactionRepo = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
        var fraudAlertRepo = scope.ServiceProvider.GetRequiredService<IFraudAlertRepository>();

        var pending = await transactionRepo.GetUnsweptForFraudAsync();
        if (pending.Count == 0) return;

        foreach (var transaction in pending)
        {
            var recentCount = await transactionRepo.CountByAccountSinceAsync(transaction.AccountId, DateTime.UtcNow.AddHours(-1));
            var (isSuspicious, reason, severity) = FraudRiskPolicy.Evaluate(transaction, recentCount);

            if (isSuspicious)
            {
                await fraudAlertRepo.AddAsync(new FraudAlert
                {
                    AccountId = transaction.AccountId,
                    TransactionId = transaction.Id,
                    Severity = severity,
                    Description = reason
                });
                logger.LogInformation("Fraud alert raised for account {AccountId}: {Reason}", transaction.AccountId, reason);
            }

            await transactionRepo.MarkSweptAsync(transaction.Id);
        }
    }
}
