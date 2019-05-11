using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutAPI.Model;
using CheckoutAPI.Model.DTO;
using CheckoutAPI.Model.Objects;
using Microsoft.EntityFrameworkCore;

namespace CheckoutAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly MockDatabaseContext _context;

        public ProductService(MockDatabaseContext context)
        {
            _context = context;
        }

        /*
         * Get Product object by id
         */
        public async Task<Product> GetProduct(long id)
        {
            return await _context.Products.FindAsync(id);
        }

        /*
         * Get all Products
         */
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        /*
         * Create new product
         */
        public async Task<GetProductViewModel> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return GetProductViewModel(product);
        }

        /*
         * Get ProductViewModel by id of the required Product
         */
        public async Task<GetProductViewModel> GetProductViewModel(long id)
        {
            var product = await GetProduct(id);

            return GetProductViewModel(product);
        }

        /*
         * Get ProductViewModel for the required Product
         */
        public GetProductViewModel GetProductViewModel(Product product)
        {
            if (product == null)
            {
                return null;
            }

            var productViewModel = new GetProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            return productViewModel;
        }

        /*
         * Get ProduceViewModels for all Products
         */
        public async Task<IEnumerable<GetProductViewModel>> GetAllProductsViewModels()
        {
            var products = await GetProducts();

            return products.Select(o => new GetProductViewModel { Id = o.Id,
                                                                  Name = o.Name,
                                                                  Price = o.Price });
        }

        /*
         * Get BasketProduceViewModels for all produces held in a Basket
         * Returns products and quantities        
         */
        public async Task<IEnumerable<GetBasketProductViewModel>> GetBasketProductViewModels(Basket basket)
        {
            var basketProducts = await GetBasketProducts(basket);

            var products = basketProducts.Join(
                _context.Products,
                basketProduct => basketProduct.Product.Id,
                product => product.Id,
                (basketProduct, product) => (
                    BasketProductId: basketProduct.Id,
                    Quantity: basketProduct.Quantity,
                    ProductId: product.Id,
                    Name: product.Name,
                    Price: product.Price
                )).Select(o => new GetBasketProductViewModel { Id = o.BasketProductId,
                                                               Quantity = o.Quantity,
                                                               Product = new GetProductViewModel { Id = o.ProductId,
                                                                                                   Name = o.Name,
                                                                                                   Price = o.Price } });

            return products;
        }

        /*
        * Get BasketProduct object by id
        */
        public async Task<BasketProduct> GetBasketProduct(long id)
        {
            return await _context.BasketProducts.FindAsync(id);
        }

        /*
         * Get all BasketProduct objects for a basket
         */
        public async Task<IEnumerable<BasketProduct>> GetBasketProducts(Basket basket)
        {
            return await _context.BasketProducts.Where(o => o.Basket.Equals(basket)).ToListAsync();
        }

        /*
         * Add a new BasketProduct
         */
        public async void AddBasketProduct(BasketProduct basketProduct)
        {
            _context.BasketProducts.Add(basketProduct);

            await _context.SaveChangesAsync();
        }

        /*
         * Update an existing BasketProduct
         */
        public async void UpdateBasketProduct(BasketProduct basketProduct)
        {
            _context.Entry(basketProduct).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        /*
         * Delete specific BasketProduct
         */
        public async void DeleteBasketProduct(BasketProduct basketProduct)
        {
            _context.BasketProducts.Remove(basketProduct);
            await _context.SaveChangesAsync();
        }

        /*
         * Delete all BasketProducts in the list
         */
        public async void DeleteBasketProducts(IEnumerable<BasketProduct> basketProducts)
        {
            foreach (var basketProduct in basketProducts)
            {
                _context.BasketProducts.Remove(basketProduct);
            }

            await _context.SaveChangesAsync();
        }
    }
}
