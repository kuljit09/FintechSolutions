using Banking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController(IAccountService accountService, ITransactionService transactionService, IFraudAlertService fraudAlertService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] Guid customerId)
    {
        var account = await accountService.GetAccountBalanceAsync(customerId, id);
        return account is null ? NotFound() : Ok(account);
    }

    [HttpGet("{id:guid}/transactions")]
    public async Task<IActionResult> GetTransactions(Guid id) => Ok(await transactionService.GetRecentTransactionsAsync(id));

    [HttpGet("{id:guid}/fraud-alerts")]
    public async Task<IActionResult> GetFraudAlerts(Guid id) => Ok(await fraudAlertService.GetAlertsForAccountAsync(id));
}
