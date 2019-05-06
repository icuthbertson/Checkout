using System;
using System.Threading.Tasks;
using CheckoutAPI.Model;
using CheckoutAPI.Model.DTO;
using CheckoutAPI.Model.Objects;

namespace CheckoutAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly MockDatabaseContext _context;
        private readonly IBasketService _basketService;

        public CustomerService(MockDatabaseContext context, IBasketService basketService)
        {
            _context = context;
            _basketService = basketService;
        }

        /*
         * Get Customer object by id 
         */
        public Task<Customer> GetCustomer(long id)
        {
            return _context.Customers.FindAsync(id);
        }

        /*
         * Get CustomerViewModel by the id of the required Customer
         */
        public async Task<GetCustomerViewModel> GetCustomerViewModel(long id)
        {
            var customer = await GetCustomer(id);

            return await GetCustomerViewModel(customer);
        }

        /*
         * Get CustomerViewModel for the required Customer
         */
        public async Task<GetCustomerViewModel> GetCustomerViewModel(Customer customer)
        {
            if (customer == null)
            {
                return null;
            }

            var basketViewModel = await _basketService.GetBasketViewModel(customer);

            var customerViewModel = new GetCustomerViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Basket = basketViewModel
            };

            return customerViewModel;
        }
    }
}
