﻿using System;
using System.Threading.Tasks;
using CheckoutAPI.Model.DTO;
using CheckoutAPI.Model.Objects;
using System.Collections.Generic;

namespace CheckoutAPI.Services
{
    public interface IProductService
    {
        Task<Product> GetProduct(long id);
        Task<IEnumerable<Product>> GetProducts();
        Task<GetProductViewModel> CreateProduct(Product product);

        Task<GetProductViewModel> GetProductViewModel(long id);
        GetProductViewModel GetProductViewModel(Product product);
        Task<IEnumerable<GetProductViewModel>> GetAllProductsViewModels();
        Task<IEnumerable<GetBasketProductViewModel>> GetBasketProductViewModels(Basket basket);

        Task<BasketProduct> GetBasketProduct(long id);
        Task<IEnumerable<BasketProduct>> GetBasketProducts(Basket basket);
        void AddBasketProduct(BasketProduct basketProduct);
        void UpdateBasketProduct(BasketProduct basketProduct);
        void DeleteBasketProduct(BasketProduct basketProduct);
        void DeleteBasketProducts(IEnumerable<BasketProduct> basketProducts);
    }
}
