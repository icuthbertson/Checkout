using System;
using System.Text;
using System.Runtime.Serialization;

namespace Checkout.Model.Objects
{
    [DataContract(Name = "product")]
    public class Product
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} for {1}\n", Name, Price);

            return sb.ToString();
        }
    }
}
