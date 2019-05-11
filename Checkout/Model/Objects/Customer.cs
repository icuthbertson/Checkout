using System;
using System.Runtime.Serialization;

namespace Checkout.Model.Objects
{
    [DataContract(Name = "customer")]
    public class Customer
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "basket")]
        public Basket Basket { get; set; }
    }
}
