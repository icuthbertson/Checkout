using System;

namespace CheckoutAPI.Model.Objects
{
    // Customer object with relationship to many Baskets
    public class Customer
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
