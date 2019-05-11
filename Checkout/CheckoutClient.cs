using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Checkout.Model.Objects;

namespace Checkout
{
    public class CheckoutClient
    {
        private readonly string _uri;
        private readonly string _customerEndpoint = "customer/{customerId}";
        private readonly string _basketEndpoint = "basket/{basketId}";
        private readonly string _basketProductsEndpoint = "basket/{basketId}/products/{basketProductId}";
        private readonly string _productEndpoint = "product/{productId}";
        private readonly DataContractJsonSerializer _customerSerializer;
        private readonly DataContractJsonSerializer _basketSerializer;
        private readonly DataContractJsonSerializer _basketProductSerializer;
        private readonly DataContractJsonSerializer _productSerializer;
        private readonly DataContractJsonSerializer _productsSerializer;

        public CheckoutClient(string uri)
        {
            _uri = uri;
            _customerSerializer = new DataContractJsonSerializer(typeof(Customer));
            _basketSerializer = new DataContractJsonSerializer(typeof(Basket));
            _basketProductSerializer = new DataContractJsonSerializer(typeof(BasketProduct));
            _productSerializer = new DataContractJsonSerializer(typeof(Product));
            _productsSerializer = new DataContractJsonSerializer(typeof(IEnumerable<Product>));
        }

        // get a customer by its id
        public async Task<Customer> GetCustomer(long customerId)
        {
            var endpoint = string.Format(_uri + _customerEndpoint, customerId);
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var client = new HttpClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var customer = await JsonToObject<Customer>(_customerSerializer, response);

                return customer;
            }

            return null;
        }

        // create a new customer
        public async Task<Customer> CreateCustomer(string name)
        {
            var endpoint = string.Format(_uri + _customerEndpoint, "");

            Customer customer = new Customer { Name = name };

            var content = new StringContent(ObjectToJson(_customerSerializer, customer), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = content
            };

            var client = new HttpClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                customer = await JsonToObject<Customer>(_customerSerializer, response);

                return customer;
            }

            return null;
        }

        // get a basket by its id
        public async Task<Basket> GetBasket(long basketId)
        {
            var endpoint = string.Format(_uri + _basketEndpoint, basketId);
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var client = new HttpClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var basket = await JsonToObject<Basket>(_basketSerializer, response);

                return basket;
            }

            return null;
        }

        // add a quantity of a product to a basket
        public async Task<Basket> AddProductToBasket(Basket basket, long quantity, Product product)
        {
            var endpoint = string.Format(_uri + _basketProductsEndpoint, basket.Id, "");

            BasketProduct basketProduct = new BasketProduct { Quantity = quantity, Product = product };

            var content = new StringContent(ObjectToJson(_basketProductSerializer, basketProduct), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = content
            };

            var client = new HttpClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                basket = await JsonToObject<Basket>(_basketSerializer, response);

                return basket;
            }

            return null;
        }

        // update the quantity of a product in a basket
        public async Task<Basket> UpdateQuantityOfProductInBasket(Basket basket, BasketProduct basketProduct)
        {
            var endpoint = string.Format(_uri + _basketProductsEndpoint, basket.Id, "");

            var content = new StringContent(ObjectToJson(_basketProductSerializer, basketProduct), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = content
            };

            var client = new HttpClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                basket = await JsonToObject<Basket>(_basketSerializer, response);

                return basket;
            }

            return null;
        }

        // empty a basket
        public async Task<Basket> EmptyBasket(Basket basket)
        {
            var endpoint = string.Format(_uri + _basketProductsEndpoint, basket.Id, "");
            var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);

            var client = new HttpClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                basket = await JsonToObject<Basket>(_basketSerializer, response);

                return basket;
            }

            return null;
        }

        // remove a product from a basket
        public async Task<Basket> RemoveProductFromBasket(Basket basket, BasketProduct basketProduct)
        {
            var endpoint = string.Format(_uri + _basketProductsEndpoint, basket.Id, basketProduct.Id);
            var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);

            var client = new HttpClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                basket = await JsonToObject<Basket>(_basketSerializer, response);

                return basket;
            }

            return null;
        }

        // get all products
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var endpoint = string.Format(_uri + _productEndpoint, "");
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var client = new HttpClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var products = await JsonToObject<IEnumerable<Product>>(_productsSerializer, response);

                return products;
            }

            return null;
        }

        // get a product by its id
        public async Task<Product> GetProduct(long productId)
        {
            var endpoint = string.Format(_uri + _productEndpoint, productId);
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var client = new HttpClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var product = await JsonToObject<Product>(_productSerializer, response);

                return product;
            }

            return null;
        }

        // create a new product
        public async Task<Product> CreateProduct(string name, double price)
        {
            var endpoint = string.Format(_uri + _productEndpoint, "");

            Product product = new Product { Name = name, Price = price };

            var content = new StringContent(ObjectToJson(_productSerializer, product), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = content
            };

            var client = new HttpClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                product = await JsonToObject<Product>(_productSerializer, response);

                return product;
            }

            return null;
        }

        // helper methods

        // generic method to convert object to Json string
        private string ObjectToJson<T>(DataContractJsonSerializer serializer, T obj)
        {
            MemoryStream ms = new MemoryStream();

            serializer.WriteObject(ms, obj);

            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        // generic method to convert Json string in HTTP response to object
        private async Task<T> JsonToObject<T>(DataContractJsonSerializer serializer, HttpResponseMessage response)
        {
            var obj = serializer.ReadObject(await response.Content.ReadAsStreamAsync());

            return (T)Convert.ChangeType(obj, typeof(T)); ;
        }
    }
}
