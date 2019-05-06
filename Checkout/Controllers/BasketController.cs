using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using CheckoutAPI.Model.Objects;
using CheckoutAPI.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

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
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            // validate product exists
            var products = await _productService.GetProducts();

            if (!products.Select(o => o.Id).Contains(basketProduct.Product.Id))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var basketProducts = await _productService.GetBasketProducts(basket);

            // if the product already exists in the basket add the new amount to the existing
            if (basketProducts.Select(o => o.Product.Id).Contains(basketProduct.Product.Id))
            {
                try
                {
                    var existingBasketProduct = basketProducts.FirstOrDefault(o => o.Product.Id.Equals(basketProduct.Product.Id));
                    if (existingBasketProduct == null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }

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
    }
}
