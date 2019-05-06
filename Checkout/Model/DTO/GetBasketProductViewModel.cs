using System;

namespace CheckoutAPI.Model.DTO
{
    public class GetBasketProductViewModel
    {
        public virtual long Id { get; set; }
        public virtual long Quantity { get; set; }
        public virtual GetProductViewModel Product { get; set; }
    }
}
