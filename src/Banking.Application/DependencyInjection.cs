using Banking.Application.Interfaces.Services;
using Banking.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<ILoanService, LoanService>();
        services.AddScoped<IDisputeService, DisputeService>();
        services.AddScoped<IFraudAlertService, FraudAlertService>();
        services.AddScoped<ISupportTicketService, SupportTicketService>();
        services.AddScoped<IKnowledgeBaseService, KnowledgeBaseService>();
        return services;
    }
}
