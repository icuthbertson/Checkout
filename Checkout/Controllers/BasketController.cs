using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using CheckoutAPI.Model.Objects;
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

        // get an individual basket
        // GET: api/basket/2
        [HttpGet("{id}")]
        public async Task<ActionResult> GetBacket(long id)
        {
            var basketViewModel = await _basketService.GetBasketViewModel(id);

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
            var basket = await _basketService.GetBasket(id);

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

        // add a new item to a basket or increase the quantity of a product already in a basket
        // POST api/basket/3/products
        [HttpPost("{id}/products")]
        public async Task<ActionResult> PostBasketProduct(long id, BasketProduct basketProduct)
        {
            var basket = await _basketService.GetBasket(id);

            if (basket == null)
            {
                return new JsonResult("Basket with id '" + id + "' does not exist")
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            if (basketProduct.Quantity < 1)
            {
                return new JsonResult("Quantity most be 1 or greater")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            // validate product exists
            var products = await _productService.GetProducts();

            if (!products.Select(o => o.Id).Contains(basketProduct.Product.Id))
            {
                return new JsonResult("Product with id '" + id + "' does not exist")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            if (!basketProduct.Basket.Id.Equals(basket.Id))
            {
                return new JsonResult("Basket with id '" + id + "' does not match the Basket referenced by the Basket Product with id '" + basketProduct.Id + "'")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            var existingBasketProduct = await _productService.GetBasketProduct(basketProduct.Id);

            // if the product already exists in the basket add the new amount to the existing
            if (existingBasketProduct != null)
            {
                try
                {
                    existingBasketProduct.Quantity += basketProduct.Quantity;
                    _productService.UpdateBasketProduct(existingBasketProduct);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                try
                {
                    basketProduct.Basket = basket;
                    _productService.AddBasketProduct(basketProduct);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        // update the quantity of a product already in the basket
        // PUT api/basket/2
        [HttpPut("{id}/products")]
        public async Task<ActionResult> UpdateBasketProduct(long id, BasketProduct basketProduct)
        {
            var basket = await _basketService.GetBasket(id);

            if (basket == null)
            {
                return new JsonResult("Basket with id '" + id + "' does not exist")
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            if (basketProduct.Quantity < 1)
            {
                return new JsonResult("Quantity most be 1 or greater")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            // validate product exists
            var products = await _productService.GetProducts();

            if (!products.Select(o => o.Id).Contains(basketProduct.Product.Id))
            {
                return new JsonResult("Product with id '" + id + "' does not exist")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            if (!basketProduct.Basket.Id.Equals(basket.Id))
            {
                return new JsonResult("Basket with id '" + id + "' does not match the Basket referenced by the Basket Product with id '" + basketProduct.Id + "'")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            var existingBasketProduct = await _productService.GetBasketProduct(basketProduct.Id);

            if (existingBasketProduct == null)
            {
                return new JsonResult("BasketProduct with id '" + basketProduct.Id + "'  does not already exist so cannot be updated")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            try 
            {             
                existingBasketProduct.Quantity = basketProduct.Quantity;
                _productService.UpdateBasketProduct(existingBasketProduct);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        // delete all products from a basket
        // DELETE api/basket/4/products
        [HttpDelete("{id}/products")]
        public async Task<ActionResult> DeleteBasketProducts(long id)
        {
            var basket = await _basketService.GetBasket(id);

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

        // delete specific product from a basket
        // DELETE api/basket/4/products/2
        [HttpDelete("{basketId}/products/{basketProductId}")]
        public async Task<ActionResult> DeleteBasketProduct(long basketId, long basketProductId)
        {
            var basket = await _basketService.GetBasket(basketId);

            if (basket == null)
            {
                return new JsonResult("Basket with id '" + basketId + "' does not exist")
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var basketProduct = await _productService.GetBasketProduct(basketProductId);
            if (basketProduct == null)
            {
                return new JsonResult("BasketProduct with id '" + basketProductId + "' does not exist")
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            if (!basketProduct.Basket.Id.Equals(basket.Id))
            {
                return new JsonResult("Basket with id '" + basketId + "' does not match the Basket referenced by the Basket Product with id '" + basketProductId + "'")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            try
            {
                _productService.DeleteBasketProduct(basketProduct);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
