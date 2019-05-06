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
        public async Task<Basket> GetBacket(long id)
        {
            return await _context.Baskets.FindAsync(id);
        }

        /*
         * Get Basket object by the associated Customer
         */
        public async Task<Basket> GetBacket(Customer customer)
        {
            return await _context.Baskets.Where(o => o.Customer.Equals(customer)).FirstOrDefaultAsync();
        }

        /*
         * Get BasketViewModel by id of the Basket
         */
        public async Task<GetBasketViewModel> GetBacketViewModel(long id)
        {
            return await GetBacketViewModel(await GetBacket(id));
        }

        /*
         * Get BasketViewModel by the associated Customer
         */
        public async Task<GetBasketViewModel> GetBacketViewModel(Customer customer)
        {
            return await GetBacketViewModel(await GetBacket(customer));
        }

        /*
         * Get the BasketViewModel from the Required Basket
         */
        public async Task<GetBasketViewModel> GetBacketViewModel(Basket basket)
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
