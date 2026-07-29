using Banking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController(ICustomerService customerService, IAccountService accountService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await customerService.GetByIdAsync(id);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpGet("{id:guid}/accounts")]
    public async Task<IActionResult> GetAccounts(Guid id) => Ok(await accountService.GetCustomerAccountsAsync(id));
}
