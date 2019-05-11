using System;
using Microsoft.EntityFrameworkCore;
using CheckoutAPI.Model.Objects;

namespace CheckoutAPI.Model
{
    // Context to be used by an in memory database
    public class MockDatabaseContext : DbContext
    {
        public MockDatabaseContext(DbContextOptions<MockDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<BasketProduct> BasketProducts { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
