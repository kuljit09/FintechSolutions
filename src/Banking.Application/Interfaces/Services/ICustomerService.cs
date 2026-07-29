using Banking.Application.DTOs;

namespace Banking.Application.Interfaces.Services;

public interface ICustomerService
{
    Task<CustomerDto?> GetByIdAsync(Guid id);
}
