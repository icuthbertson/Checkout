using System;
using System.Threading.Tasks;
using CheckoutAPI.Model.DTO;
using CheckoutAPI.Model.Objects;

namespace CheckoutAPI.Services
{
    public interface IBasketService
    {
        Task<Basket> GetBacket(long id);
        Task<Basket> GetBacket(Customer customer);

        Task<GetBasketViewModel> GetBacketViewModel(long id);
        Task<GetBasketViewModel> GetBacketViewModel(Customer customer);
        Task<GetBasketViewModel> GetBacketViewModel(Basket basket);
    }
}
