using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Checkout.Model.Objects
{
    [DataContract(Name = "basket")]
    public class Basket
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "products")]
        public IEnumerable<BasketProduct> BasketProducts { get; set; }
    }
}
