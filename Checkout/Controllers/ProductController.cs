using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using CheckoutAPI.Model;

namespace CheckoutAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly BasketContext _context;

        public ProductController(BasketContext context)
        {
            _context = context;
        }

        // required methods
        // GET / all products
        // GET {id}/ individual products
        // PUT {id}/ update product info
        // POST / create new product
        // DELETE {id}/ delete product
    }
}
