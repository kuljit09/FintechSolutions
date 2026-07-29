using System.ComponentModel;
using Banking.Application.DTOs;
using Banking.Application.Interfaces.Services;

namespace Banking.McpServer.Tools;

/// <summary>ToolRiskTier.LowRiskWrite.</summary>
public class SupportTicketTools(ISupportTicketService ticketService)
{
    [Description("Creates a support ticket when the chatbot cannot resolve the customer's issue on its own.")]
    public async Task<SupportTicketDto> CreateSupportTicket(Guid customerId, Guid? accountId, string subject, string description)
        => await ticketService.CreateAsync(new CreateSupportTicketRequest(customerId, accountId, subject, description));
}
