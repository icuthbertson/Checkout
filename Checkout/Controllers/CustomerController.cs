using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using CheckoutAPI.Model;
using System.Threading.Tasks;

namespace CheckoutAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly BasketContext _context;

        public CustomerController(BasketContext context)
        {
            _context = context;
        }

        // required methods
        // GET {id} customer
        // GET {id}/baskets get all baskets (if site could handle multiple baskets)
        // GET {id}/baskets/{id} get individual basket
        // DELETE {id}/baskets/{id} delete individual basket

        // GET: api/Customer/3
        [HttpGet("id")]
        public async Task<ActionResult<Customer>> GetCustomer(long id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }
    }
}
