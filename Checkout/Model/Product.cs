using System;

namespace CheckoutAPI.Model
{
    // Product object that defines products and their prices
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
    }
}
