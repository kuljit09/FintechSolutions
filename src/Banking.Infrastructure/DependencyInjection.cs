using Banking.Application.Interfaces.Repositories;
using Banking.Application.Interfaces.Services;
using Banking.Infrastructure.BackgroundServices;
using Banking.Infrastructure.Persistence;
using Banking.Infrastructure.Repositories;
using Banking.Infrastructure.VectorSearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres")
            ?? "Host=localhost;Port=5432;Database=banking_genai;Username=postgres;Password=devpass";

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString, o => o.UseVector())); // verify UseVector() against installed Pgvector.EntityFrameworkCore version

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<IDisputeRepository, DisputeRepository>();
        services.AddScoped<IFraudAlertRepository, FraudAlertRepository>();
        services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
        services.AddScoped<IKnowledgeBaseRepository, KnowledgeBaseRepository>();

        var ollamaBaseUrl = configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
        services.AddHttpClient<IEmbeddingGenerator, OllamaEmbeddingGenerator>(c =>
        {
            c.BaseAddress = new Uri(ollamaBaseUrl);
            c.Timeout = TimeSpan.FromSeconds(60);
        });

        // The "near real-time" background fraud sweep - registered as a hosted service so it
        // starts automatically with the API host and runs independently of any HTTP request.
        services.AddHostedService<FraudSweepBackgroundService>();

        return services;
    }
}
