using System;
using System.Collections.Generic;

namespace CheckoutAPI.Model.DTO
{
    public class GetBasketViewModel
    {
        public virtual long Id { get; set; }
        public virtual IEnumerable<GetProductViewModel> Products { get; set; }
    }
}
