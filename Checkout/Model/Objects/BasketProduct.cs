using System;
using System.Runtime.Serialization;

namespace Checkout.Model.Objects
{
    [DataContract(Name = "products")]
    public class BasketProduct
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "quantity")]
        public long Quantity { get; set; }

        [DataMember(Name = "product")]
        public Product Product { get; set; }
    }
}
