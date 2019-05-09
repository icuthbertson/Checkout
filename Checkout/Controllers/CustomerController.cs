using Microsoft.AspNetCore.Mvc;
using System;
using CheckoutAPI.Model.Objects;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using CheckoutAPI.Services;
using CheckoutAPI.Model.DTO;

namespace CheckoutAPI.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

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

        // create a new customer
        // POST: api/customer
        [HttpPost]
        public async Task<ActionResult> PostCustomer(Customer customer)
        {
            GetCustomerViewModel customerViewModel;

            if (string.IsNullOrEmpty(customer.Name))
            {
                return new JsonResult("Customer is missing 'Name' field")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            try
            {
                customerViewModel = await _customerService.CreateCustomer(customer);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return new JsonResult(customerViewModel) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
