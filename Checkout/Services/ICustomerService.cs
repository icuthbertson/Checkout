using System;
using System.Threading.Tasks;
using CheckoutAPI.Model.DTO;
using CheckoutAPI.Model.Objects;

namespace CheckoutAPI.Services
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomer(long id);

        Task<GetCustomerViewModel> GetCustomerViewModel(long id);
        Task<GetCustomerViewModel> GetCustomerViewModel(Customer customer);
    }
}
