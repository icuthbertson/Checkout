using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using CheckoutAPI.Model.Objects;
using System.Threading.Tasks;
using CheckoutAPI.Services;
using Microsoft.AspNetCore.Http;

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

        // required methods
        // GET / all products
        // GET {id}/ individual products
        // PUT {id}/ update product info
        // POST / create new product
        // DELETE {id}/ delete product

        // get all products
        // GET: api/product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var allProductsViewModels = await _productService.GetProducts();

            return new JsonResult(allProductsViewModels) { StatusCode = StatusCodes.Status200OK };
        }

        //// get individual product
        //// GET: api/product/7
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            var productViewModel = await _productService.GetProductViewModel(id);

            if (productViewModel == null)
            {
                return new JsonResult("Product with id '" + id + "' does not exist") { StatusCode = StatusCodes.Status404NotFound };
            }

            return new JsonResult(productViewModel) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
