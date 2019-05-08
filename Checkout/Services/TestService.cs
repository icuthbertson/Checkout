using System;
using CheckoutAPI.Model;
using CheckoutAPI.Model.Objects;

namespace CheckoutAPI.Services
{
    public class TestService : ITestService
    {
        private readonly MockDatabaseContext _context;

        public TestService(MockDatabaseContext context)
        {
            _context = context;
        }

        public async void CreateTestCustomer()
        {
            // create test customer, basket and two products
            var customer = new Customer { Name = "Test Customer" };
            var basket = new Basket { Customer = customer };
            var product1 = new Product { Name = "Product1", Price = 10.00f };
            var product2 = new Product { Name = "Product2", Price = 5.99f };
            var basketProduct1 = new BasketProduct { Basket = basket, Product = product1, Quantity = 1 };
            var basketProduct2 = new BasketProduct { Basket = basket, Product = product2, Quantity = 2 };

            _context.Customers.Add(customer);
            _context.Baskets.Add(basket);
            _context.Products.Add(product1);
            _context.Products.Add(product2);
            _context.BasketProducts.Add(basketProduct1);
            _context.BasketProducts.Add(basketProduct2);
            await _context.SaveChangesAsync();
        }
    }
}
