using Banking.AI.KernelSetup;
using Banking.AI.Memory;
using Banking.AI.Orchestration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.AI;

public static class DependencyInjection
{
    public static IServiceCollection AddBankingAI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBankingSemanticKernel(configuration);

        services.AddSingleton<ChatHistoryStore>();
        services.AddSingleton<ConversationContext>();
        services.AddSingleton<PendingConfirmationStore>();
        services.AddScoped<HighRiskActionExecutor>();
        services.AddScoped<IChatOrchestrator, ChatOrchestrator>();

        return services;
    }
}
