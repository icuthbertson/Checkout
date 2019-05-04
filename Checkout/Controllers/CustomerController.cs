using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using CheckoutAPI.Model.Objects;
using CheckoutAPI.Model;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using CheckoutAPI.Model.DTO;

namespace CheckoutAPI.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly MockDatabaseContext _context;

        public CustomerController(MockDatabaseContext context)
        {
            _context = context;

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

        // create a new customer
        // POST: api/customer
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);

            // customers will always have at least one basket
            _context.Baskets.Add(new Basket { Customer = customer });

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        // get all customers
        // GET: api/customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // get an individual customer
        // GET: api/customer/3
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomer(long id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var basket = await _context.Baskets.Where(o => o.Customer.Equals(customer)).FirstOrDefaultAsync();

            if (basket == null)
            {
                return NotFound();
            }

            var basketProducts = await _context.BasketProducts.Where(o => o.Basket.Equals(basket)).ToListAsync();

            var products = basketProducts.Join(
                _context.Products,
                basketProduct => basketProduct.Product.Id,
                product => product.Id,
                (basketProduct, product) => (
                    ProductId: product.Id,
                    Name: product.Name,
                    Quantity: basketProduct.Quantity,
                    Price: product.Price
                ));

            var productViewModels = new List<GetProductViewModel>();
            foreach (var product in products)
            {
                productViewModels.Add(new GetProductViewModel { Id = product.ProductId,
                                                                Name = product.Name, 
                                                                Quantity = product.Quantity, 
                                                                Price = product.Price });
            }

            var basketViewModel = new GetBasketViewModel { Id = basket.Id,
                                                           Products = productViewModels };
            var customerViewModel = new GetCustomerViewModel { Id = customer.Id,
                                                               Name = customer.Name,
                                                               Basket = basketViewModel };

            return new JsonResult(customerViewModel) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
