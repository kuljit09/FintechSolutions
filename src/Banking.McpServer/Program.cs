using Banking.Application;
using Banking.Infrastructure;
using Banking.McpServer.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration); // NOTE: this also registers the fraud sweep hosted service - fine to run in both Api and McpServer during learning, but in a real deployment run it in exactly one place.

builder.Services.AddScoped<AccountTools>();
builder.Services.AddScoped<TransactionTools>();
builder.Services.AddScoped<CardTools>();
builder.Services.AddScoped<LoanTools>();
builder.Services.AddScoped<DisputeTools>();
builder.Services.AddScoped<FraudAlertTools>();
builder.Services.AddScoped<KnowledgeBaseTools>();
builder.Services.AddScoped<SupportTicketTools>();

// --------------------------------------------------------------------------------------------
// MCP server registration - PSEUDOCODE / reference only, same caveat as the e-commerce project:
//
//   builder.Services.AddMcpServer()
//       .WithStdioServerTransport()          // or .WithHttpServerTransport()
//       .WithTools<AccountTools>()
//       .WithTools<TransactionTools>()
//       .WithTools<CardTools>()
//       .WithTools<LoanTools>()
//       .WithTools<DisputeTools>()
//       .WithTools<FraudAlertTools>()
//       .WithTools<KnowledgeBaseTools>()
//       .WithTools<SupportTicketTools>();
//
// VERIFY the current ModelContextProtocol C# SDK API before wiring this up for real.
// --------------------------------------------------------------------------------------------

var host = builder.Build();
await host.RunAsync();
