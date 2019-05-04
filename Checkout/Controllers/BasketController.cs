using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using CheckoutAPI.Model;
using System.Threading.Tasks;
using System.Linq;

namespace CheckoutAPI.Controllers
{
    [Route("api/basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly MockDatabaseContext _context;

        public BasketController(MockDatabaseContext context)
        {
            _context = context;
        }

        // required methods
        // GET {id}/ individual basket
        // POST {id}/ add products to basket
        // DELETE {id}/ delete basket
        // GET {id}/products all products in a basket
        // PUT {id}/products update products in basket
        // DELETE {id}/products delete all products in basket

        // 
    }
}
