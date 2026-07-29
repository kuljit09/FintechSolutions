using Banking.AI.McpClient;
using Banking.AI.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace Banking.AI.KernelSetup;

public static class KernelBuilderExtensions
{
    public static IServiceCollection AddBankingSemanticKernel(this IServiceCollection services, IConfiguration configuration)
    {
        var ollamaBaseUrl = configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
        var chatModel = configuration["Ollama:ChatModel"] ?? "llama3.1";

        services.AddSingleton<IMcpToolClient, McpToolClient>();
        services.AddTransient<AccountPlugin>();
        services.AddTransient<TransactionPlugin>();
        services.AddTransient<CardPlugin>();
        services.AddTransient<LoanPlugin>();
        services.AddTransient<DisputePlugin>();
        services.AddTransient<FraudAlertPlugin>();
        services.AddTransient<SupportTicketPlugin>();
        services.AddTransient<KnowledgeBasePlugin>();

        services.AddTransient(sp =>
        {
            var builder = Kernel.CreateBuilder();

            // NOTE: verify exact method name/signature against your installed Ollama connector version.
            builder.AddOllamaChatCompletion(modelId: chatModel, endpoint: new Uri(ollamaBaseUrl));

            builder.Plugins.AddFromObject(sp.GetRequiredService<AccountPlugin>(), "AccountPlugin");
            builder.Plugins.AddFromObject(sp.GetRequiredService<TransactionPlugin>(), "TransactionPlugin");
            builder.Plugins.AddFromObject(sp.GetRequiredService<CardPlugin>(), "CardPlugin");
            builder.Plugins.AddFromObject(sp.GetRequiredService<LoanPlugin>(), "LoanPlugin");
            builder.Plugins.AddFromObject(sp.GetRequiredService<DisputePlugin>(), "DisputePlugin");
            builder.Plugins.AddFromObject(sp.GetRequiredService<FraudAlertPlugin>(), "FraudAlertPlugin");
            builder.Plugins.AddFromObject(sp.GetRequiredService<SupportTicketPlugin>(), "SupportTicketPlugin");
            builder.Plugins.AddFromObject(sp.GetRequiredService<KnowledgeBasePlugin>(), "KnowledgeBasePlugin");

            return builder.Build();
        });

        return services;
    }
}
