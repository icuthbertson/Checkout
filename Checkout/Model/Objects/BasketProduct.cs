using System;
using System.Text;
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

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Quantity: {0}\n", Quantity);
            sb.Append(Product);

            return sb.ToString();
        }
    }
}
