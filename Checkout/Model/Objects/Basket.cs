using System;
using System.Text;
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

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Products in basket\n");

            foreach(var basketProduct in BasketProducts)
            {
                sb.Append(basketProduct);
            }

            return sb.ToString();
        }
    }
}
