using System;
using System.Threading.Tasks;
using CheckoutAPI.Model.DTO;
using CheckoutAPI.Model.Objects;

namespace CheckoutAPI.Services
{
    public interface IBasketService
    {
        Task<Basket> GetBasket(long id);
        Task<Basket> GetBasket(Customer customer);

        Task<GetBasketViewModel> GetBasketViewModel(long id);
        Task<GetBasketViewModel> GetBasketViewModel(Customer customer);
        Task<GetBasketViewModel> GetBasketViewModel(Basket basket);
    }
}
