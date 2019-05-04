using System;

namespace CheckoutAPI.Model.DTO
{
    public class GetCustomerViewModel
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual GetBasketViewModel Basket { get; set; }
    }
}
