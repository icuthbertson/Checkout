using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using CheckoutAPI.Model.Objects;
using System.Threading.Tasks;
using CheckoutAPI.Services;
using Microsoft.AspNetCore.Http;
using CheckoutAPI.Model.DTO;

namespace CheckoutAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /* get all products
         * GET: api/product
         * RESPONSE: 200 OK
         * [
         *      {
         *          "id": 1,
         *          "name": "Product 3",
         *          "price": 10.75
         *      },
         *      {
         *          "id": 2,
         *          "name": "Product 2",
         *          "price": 10
         *      },
         *      {
         *          "id": 3,
         *          "name": "Product 1",
         *          "price": 5.99
         *      }
         * ]
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var allProductsViewModels = await _productService.GetProducts();

            return new JsonResult(allProductsViewModels) { StatusCode = StatusCodes.Status200OK };
        }

        /* get individual product
         * GET: api/product/7
         * RESPONSE: 200 OK
         * {
         *   "id": 1,
         *   "name": "Product 3",
         *   "price": 10.75
         * }
         */
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            var productViewModel = await _productService.GetProductViewModel(id);

            if (productViewModel == null)
            {
                return new JsonResult("Product with id '" + id + "' does not exist")
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            return new JsonResult(productViewModel) { StatusCode = StatusCodes.Status200OK };
        }

        /* create new product
         * POST: api/product
         * BODY:
         * {
         *   "name": "Product 1",
         *   "price": 5.99
         * }
         * RESPONSE: 200 OK
         * {
         *   "id": 1,
         *   "name": "Product 1",
         *   "price": 5.99
         * }       
         */
        [HttpPost]
        public async Task<ActionResult> PostProduct(Product product)
        {
            GetProductViewModel productViewModel;

            if (string.IsNullOrEmpty(product.Name))
            {
                return new JsonResult("Product is missing 'Name' field")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            if (product.Price < 0.0)
            {
                return new JsonResult("Product Price cannot be less than 0 (free)")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            try
            {
                productViewModel = await _productService.CreateProduct(product);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return new JsonResult(productViewModel) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
