using System;

namespace CheckoutAPI.Model.DTO
{
    public class GetProductViewModel
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual float Price { get; set; }
    }
}
