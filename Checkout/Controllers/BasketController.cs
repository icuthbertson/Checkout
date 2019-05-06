using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CheckoutAPI.Services;
using Microsoft.AspNetCore.Http;

namespace CheckoutAPI.Controllers
{
    [Route("api/basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        // required methods
        // GET {id}/ individual basket
        // POST {id}/ add products to basket
        // DELETE {id}/ delete basket
        // GET {id}/products all products in a basket
        // PUT {id}/products update products in basket
        // DELETE {id}/products delete all products in basket

        // get an individual basket
        // GET: api/basket/2
        [HttpGet("{id}")]
        public async Task<ActionResult> GetBacket(long id)
        {
            var basketViewModel = await _basketService.GetBacketViewModel(id);

            if (basketViewModel == null)
            {
                return new JsonResult("Basket with id '" + id + "' does not exist") { StatusCode = StatusCodes.Status404NotFound };
            }

            return new JsonResult(basketViewModel) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
