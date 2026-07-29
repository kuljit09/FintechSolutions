using Banking.Application.DTOs;
using Banking.Application.Interfaces.Repositories;
using Banking.Application.Interfaces.Services;

namespace Banking.Application.Services;

public class CustomerService(ICustomerRepository customers) : ICustomerService
{
    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        var c = await customers.GetByIdAsync(id);
        return c is null ? null : new CustomerDto(c.Id, c.FullName, c.Email, c.KycStatus.ToString(), c.CreditScore);
    }
}
