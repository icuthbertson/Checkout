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
        private readonly IProductService _productService;

        public BasketController(IBasketService basketService, IProductService productService)
        {
            _basketService = basketService;
            _productService = productService;
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
                return new JsonResult("Basket with id '" + id + "' does not exist") 
                { 
                    StatusCode = StatusCodes.Status404NotFound 
                };
            }

            return new JsonResult(basketViewModel) { StatusCode = StatusCodes.Status200OK };
        }

        // get all products and quantities held in a basket
        // GET api/basket/1/products
        [HttpGet("{id}/products")]
        public async Task<ActionResult> GetBasketProducts(long id)
        {
            var basket = await _basketService.GetBacket(id);

            if (basket == null)
            {
                return new JsonResult("Basket with id '" + id + "' does not exist") 
                { 
                    StatusCode = StatusCodes.Status404NotFound 
                };
            }

            var basketProductViewModels = await _productService.GetBasketProductViewModels(basket);

            return new JsonResult(basketProductViewModels) { StatusCode = StatusCodes.Status200OK };
        }

        // delete all products from a basket
        // DELETE api/basket/4/products
        [HttpDelete("{id}/products")]
        public async Task<ActionResult> DeleteBasketProducts(long id)
        {
            var basket = await _basketService.GetBacket(id);

            if (basket == null)
            {
                return new JsonResult("Basket with id '" + id + "' does not exist") 
                { 
                    StatusCode = StatusCodes.Status404NotFound 
                };
            }

            var basketProducts = await _productService.GetBasketProducts(basket);
            try
            {
                _productService.DeleteBasketProducts(basketProducts);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
