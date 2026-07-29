using Banking.Application.DTOs;
using Banking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Controllers;

[ApiController]
[Route("api/loans")]
public class LoansController(ILoanService loanService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] Guid customerId)
    {
        var loan = await loanService.GetLoanStatusAsync(customerId, id);
        return loan is null ? NotFound() : Ok(loan);
    }

    [HttpGet("{id:guid}/repayment-schedule")]
    public async Task<IActionResult> GetRepaymentSchedule(Guid id) => Ok(await loanService.GetRepaymentScheduleAsync(id));

    [HttpPost("apply")]
    public async Task<IActionResult> Apply([FromBody] LoanApplicationRequest request) => Ok(await loanService.ApplyAsync(request));
}
