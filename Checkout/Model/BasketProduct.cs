using System;

namespace CheckoutAPI.Model
{
    // BasketProduct object with many-to-many relationship between Baskets and Products of varying quantities
    // Basket and Product would have a unique index to ensure a Basket can only ever have a single entry for each Product
    public class BasketProduct
    {
        public long Id { get; set; }
        public Basket Basket { get; set; }
        public long Quantity { get; set; }
        public Product Product { get; set; }
    }
}
