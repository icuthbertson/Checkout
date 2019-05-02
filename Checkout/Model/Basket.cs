using System;

namespace CheckoutAPI.Model
{
    // Basket object with many-to-one relationship to a Customer
    public class Basket
    {
        public long Id { get; set; }
        public Customer Customer { get; set; }
    }
}
