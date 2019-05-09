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

        /* get an individual basket
         * GET: api/basket/2
         * RESPONSE: 200 OK
         * {
         *    "id": 1,
         *    "products": [
         *       {
         *         "id": 2,
         *         "quantity": 5,
         *         "product": {
         *           "id": 1,
         *           "name": "Product 1",
         *           "price": 6.55
         *         }
         *       },
         *       {
         *         "id": 3,
         *         "quantity": 10,
         *         "product": {
         *           "id": 2,
         *           "name": "Product 2",
         *           "price": 10
         *        }
         *    ]
         * }       
         */
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

        /* get all products and quantities held in a basket
         * GET api/basket/1/products
         * RESPONSE: 200 OK        
         * [
         *    {
         *        "id": 2,
         *        "quantity": 5,
         *        "product": {
         *           "id": 1,
         *           "name": "Product 1",
         *           "price": 6.55
         *        }
         *     },
         *     {
         *        "id": 3,
         *        "quantity": 10,
         *        "product": {
         *           "id": 2,
         *           "name": "Product 2",
         *           "price": 10
         *        }
         *    }
         * ]
         */
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

        /* add a new item to a basket or increase the quantity of a product already in a basket
         * POST api/basket/1/products
         * BODY:
         * {
         *    "quantity": 10,
         *    "product": {
         *       "id": 1
         *    }
         * }
         * REPONSE: 200 OK
         * {
         *    "id": 1,
         *    "products": [
         *       {
         *         "id": 2,
         *         "quantity": 10,
         *         "product": {
         *           "id": 1,
         *           "name": "Product 1",
         *           "price": 6.55
         *         }
         *       }
         *    ]
         * }  
         */
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

            var basketViewModel = await _basketService.GetBasketViewModel(id);
            return new JsonResult(basketViewModel) { StatusCode = StatusCodes.Status200OK };
        }

        /* update the quantity of a product already in the basket
         * PUT api/basket/1/products
         * BODY:
         * {
         *   "id": 2,
         *   "quantity": 5,
         *   "product": {
         *       "id": 1,
         *       "name": "Product 1",
         *       "price": 6.55
         *   }
         * } 
         * REPONSE: 200 OK
         * {
         *    "id": 1,
         *    "products": [
         *       {
         *         "id": 2,
         *         "quantity": 5,
         *         "product": {
         *           "id": 1,
         *           "name": "Product 1",
         *           "price": 6.55
         *         }
         *       },
         *       {
         *         "id": 3,
         *         "quantity": 10,
         *         "product": {
         *           "id": 2,
         *           "name": "Product 2",
         *           "price": 10
         *        }
         *    ]
         * }  
         */
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

            var existingBasketProduct = await _productService.GetBasketProduct(basketProduct.Id);

            if (existingBasketProduct == null)
            {
                return new JsonResult("BasketProduct with id '" + basketProduct.Id + "'  does not already exist so cannot be updated")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            if (!existingBasketProduct.Basket.Id.Equals(basket.Id))
            {
                return new JsonResult("Basket with id '" + basket.Id + "' does not match the Basket referenced by the Basket Product with id '" + existingBasketProduct.Id + "'")
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

            var basketViewModel = await _basketService.GetBasketViewModel(id);
            return new JsonResult(basketViewModel) { StatusCode = StatusCodes.Status200OK };
        }

        /* delete all products from a basket
         * DELETE api/basket/1/products
         * REPONSE: 200 OK
         * {
         *   "id": 1,
         *   "products": []
         * }        
         */
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

            var basketViewModel = await _basketService.GetBasketViewModel(id);
            return new JsonResult(basketViewModel) { StatusCode = StatusCodes.Status200OK };
        }

        /* delete specific product from a basket
         * DELETE api/basket/1/products/2
         * REPONSE: 200 OK
         * {
         *  "id": 1,
         *  "products": [
         *    {
         *        "id": 3,
         *        "quantity": 7,
         *        "product": {
         *            "id": 1,
         *            "name": "Product 3",
         *            "price": 10.75
         *        }
         *    }
         *  ]
         * }        
         */
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

            var basketViewModel = await _basketService.GetBasketViewModel(basketId);
            return new JsonResult(basketViewModel) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
