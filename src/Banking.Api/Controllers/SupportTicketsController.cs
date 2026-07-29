using Banking.Application.DTOs;
using Banking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Controllers;

[ApiController]
[Route("api/support-tickets")]
public class SupportTicketsController(ISupportTicketService ticketService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSupportTicketRequest request) => Ok(await ticketService.CreateAsync(request));
}
