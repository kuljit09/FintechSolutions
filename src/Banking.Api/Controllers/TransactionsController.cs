using Banking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionsController(ITransactionService transactionService, IDisputeService disputeService) : ControllerBase
{
    [HttpGet("{id:guid}/explain-failure")]
    public async Task<IActionResult> ExplainFailure(Guid id) => Ok(await transactionService.ExplainFailureAsync(id));

    [HttpGet("{id:guid}/dispute-eligibility")]
    public async Task<IActionResult> CheckDisputeEligibility(Guid id) => Ok(await disputeService.CheckEligibilityAsync(id));

    [HttpPost("{id:guid}/disputes")]
    public async Task<IActionResult> FileDispute(Guid id, [FromBody] FileDisputeBody body)
    {
        var dispute = await disputeService.FileDisputeAsync(new Application.DTOs.FileDisputeRequest(id, body.Reason));
        return Ok(dispute);
    }

    public record FileDisputeBody(string Reason);
}
