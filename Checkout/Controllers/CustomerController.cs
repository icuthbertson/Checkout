using Microsoft.AspNetCore.Mvc;
using System;
using CheckoutAPI.Model.Objects;
using CheckoutAPI.Model;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using CheckoutAPI.Services;

namespace CheckoutAPI.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly MockDatabaseContext _context;
        private readonly ICustomerService _customerService;

        public CustomerController(MockDatabaseContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;

            if (_context.Customers.Count() == 0)
            {
                // create test customer, basket and two products
                var customer = new Customer { Name = "Test Customer" };
                var basket = new Basket { Customer = customer };
                var product1 = new Product { Name = "Product1", Price = 10.00f };
                var product2 = new Product { Name = "Product2", Price = 5.99f };
                var basketProduct1 = new BasketProduct { Basket = basket, Product = product1, Quantity = 1 };
                var basketProduct2 = new BasketProduct { Basket = basket, Product = product2, Quantity = 2 };

                _context.Customers.Add(customer);
                _context.Baskets.Add(basket);
                _context.Products.Add(product1);
                _context.Products.Add(product2);
                _context.BasketProducts.Add(basketProduct1);
                _context.BasketProducts.Add(basketProduct2);
                _context.SaveChanges();
            }
        }

        // required methods
        // GET {id} customer
        // GET {id}/baskets get all baskets (if site could handle multiple baskets)
        // GET {id}/baskets/{id} get individual basket
        // DELETE {id}/baskets/{id} delete individual basket

        // get an individual customer
        // GET: api/customer/3
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomer(long id)
        {
            var customerViewModel = await _customerService.GetCustomerViewModel(id);

            if (customerViewModel == null)
            {
                return new JsonResult("Customer with id '" + id + "' does not exist") 
                { 
                    StatusCode = StatusCodes.Status404NotFound 
                };
            }

            return new JsonResult(customerViewModel) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
