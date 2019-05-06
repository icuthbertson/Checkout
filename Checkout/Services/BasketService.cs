using System;
using System.Linq;
using System.Threading.Tasks;
using CheckoutAPI.Model;
using CheckoutAPI.Model.DTO;
using CheckoutAPI.Model.Objects;
using Microsoft.EntityFrameworkCore;

namespace CheckoutAPI.Services
{
    public class BasketService : IBasketService
    {
        private readonly MockDatabaseContext _context;
        private readonly IProductService _productService;

        public BasketService(MockDatabaseContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        /*
         * Get Basket object by Id
         */
        public async Task<Basket> GetBasket(long id)
        {
            return await _context.Baskets.FindAsync(id);
        }

        /*
         * Get Basket object by the associated Customer
         */
        public async Task<Basket> GetBasket(Customer customer)
        {
            return await _context.Baskets.Where(o => o.Customer.Equals(customer)).FirstOrDefaultAsync();
        }

        /*
         * Get BasketViewModel by id of the Basket
         */
        public async Task<GetBasketViewModel> GetBasketViewModel(long id)
        {
            return await GetBasketViewModel(await GetBasket(id));
        }

        /*
         * Get BasketViewModel by the associated Customer
         */
        public async Task<GetBasketViewModel> GetBasketViewModel(Customer customer)
        {
            return await GetBasketViewModel(await GetBasket(customer));
        }

        /*
         * Get the BasketViewModel from the Required Basket
         */
        public async Task<GetBasketViewModel> GetBasketViewModel(Basket basket)
        {
            if (basket == null)
            {
                return null;
            }

            var basketProductViewModels = await _productService.GetBasketProductViewModels(basket);

            var basketViewModel = new GetBasketViewModel
            {
                Id = basket.Id,
                Products = basketProductViewModels
            };

            return basketViewModel;
        }
    }
}
