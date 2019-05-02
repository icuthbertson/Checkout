using System;
using Microsoft.EntityFrameworkCore;

namespace CheckoutAPI.Model
{
    // Context to be used by an in memory database
    public class BasketContext : DbContext
    {
        public BasketContext(DbContextOptions<BasketContext> options)
            : base(options)
        {
        }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<BasketProduct> BasketProducts { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
