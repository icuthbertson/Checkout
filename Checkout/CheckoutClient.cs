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
        private readonly string _customerEndpoint = "customer/{0}";
        private readonly string _basketEndpoint = "basket/{0}";
        private readonly string _basketProductsEndpoint = "basket/{0}/products/{1}";
        private readonly string _productEndpoint = "product/{0}";
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

        /*
         * get a customer by its id
         */       
        public async Task<Customer> GetCustomer(long customerId)
        {
            var endpoint = string.Format(_uri + _customerEndpoint, customerId);

            return await SendRequestAndParseReponse<Customer>(HttpMethod.Put, endpoint, _customerSerializer);
        }

        /*
         * create a new customer
         */       
        public async Task<Customer> CreateCustomer(string name)
        {
            var endpoint = string.Format(_uri + _customerEndpoint, "");

            Customer customer = new Customer { Name = name };

            var content = ObjectToJson(_customerSerializer, customer);

            return await SendRequestAndParseReponse<Customer>(HttpMethod.Post, endpoint, content, _customerSerializer);
        }

        /*
         * get a basket by its id
         */       
        public async Task<Basket> GetBasket(long basketId)
        {
            var endpoint = string.Format(_uri + _basketEndpoint, basketId);

            return await SendRequestAndParseReponse<Basket>(HttpMethod.Get, endpoint, _basketSerializer);
        }

        /*
         * add a quantity of a product to a basket
         */       
        public async Task<Basket> AddProductToBasket(Basket basket, long quantity, Product product)
        {
            var endpoint = string.Format(_uri + _basketProductsEndpoint, basket.Id, "");

            BasketProduct basketProduct = new BasketProduct { Quantity = quantity, Product = product };

            var content = ObjectToJson(_basketProductSerializer, basketProduct);

            return await SendRequestAndParseReponse<Basket>(HttpMethod.Post, endpoint, content, _basketSerializer);
        }

        /*
         * update the quantity of a product in a basket
         */       
        public async Task<Basket> UpdateQuantityOfProductInBasket(Basket basket, BasketProduct basketProduct)
        {
            var endpoint = string.Format(_uri + _basketProductsEndpoint, basket.Id, "");

            var content = ObjectToJson(_basketProductSerializer, basketProduct);

            return await SendRequestAndParseReponse<Basket>(HttpMethod.Put, endpoint, content, _basketSerializer);
        }

        /*
         * empty a basket
         */       
        public async Task<Basket> EmptyBasket(Basket basket)
        {
            var endpoint = string.Format(_uri + _basketProductsEndpoint, basket.Id, "");

            return await SendRequestAndParseReponse<Basket>(HttpMethod.Delete, endpoint, _basketSerializer);
        }

        /*
         * remove a product from a basket
         */       
        public async Task<Basket> RemoveProductFromBasket(Basket basket, BasketProduct basketProduct)
        {
            var endpoint = string.Format(_uri + _basketProductsEndpoint, basket.Id, basketProduct.Id);

            return await SendRequestAndParseReponse<Basket>(HttpMethod.Delete, endpoint, _basketSerializer);
        }

        /*
         * get all products
         */       
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var endpoint = string.Format(_uri + _productEndpoint, "");

            return await SendRequestAndParseReponse<IEnumerable<Product>>(HttpMethod.Get, endpoint, _productsSerializer);
        }

        /*
         * get a product by its id
         */       
        public async Task<Product> GetProduct(long productId)
        {
            var endpoint = string.Format(_uri + _productEndpoint, productId);

            return await SendRequestAndParseReponse<Product>(HttpMethod.Get, endpoint, _productSerializer);
        }

        /*
         * create a new product
         */       
        public async Task<Product> CreateProduct(string name, double price)
        {
            var endpoint = string.Format(_uri + _productEndpoint, "");

            Product product = new Product { Name = name, Price = price };

            var content = ObjectToJson(_productSerializer, product);

            return await SendRequestAndParseReponse<Product>(HttpMethod.Post, endpoint, content, _productSerializer);
        }

        #region helper methods

        /*
         * generic method to send http request and parse the reponse object
         */
        private async Task<T> SendRequestAndParseReponse<T>(HttpMethod httpMethod, string endpoint, DataContractJsonSerializer jsonSerializer)
        {
            return await SendRequestAndParseReponse<T>(httpMethod, endpoint, null, jsonSerializer);
        }

        /*
         * generic method to send http request and parse the reponse object
         * 
         * throws runtime excpetion if an error response code is recieved
         */       
        private async Task<T> SendRequestAndParseReponse<T>(HttpMethod httpMethod, string endpoint, StringContent content, DataContractJsonSerializer jsonSerializer)
        {
            var request = new HttpRequestMessage(httpMethod, endpoint)
            {
                Content = content
            };

            var client = new HttpClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseObj = await JsonToObject<T>(jsonSerializer, response);

                return responseObj;
            }

            throw new Exception(response.StatusCode + ": " + response.Content);
        }

        /*
         * generic method to convert object to Json string
         */       
        private StringContent ObjectToJson<T>(DataContractJsonSerializer serializer, T obj)
        {
            MemoryStream ms = new MemoryStream();

            serializer.WriteObject(ms, obj);

            byte[] json = ms.ToArray();
            ms.Close();
            
            return new StringContent(Encoding.UTF8.GetString(json, 0, json.Length), Encoding.UTF8, "application/json");
        }

        /*
         * generic method to convert Json string in HTTP response to object
         */       
        private async Task<T> JsonToObject<T>(DataContractJsonSerializer serializer, HttpResponseMessage response)
        {
            var obj = serializer.ReadObject(await response.Content.ReadAsStreamAsync());

            return (T)Convert.ChangeType(obj, typeof(T)); ;
        }

        #endregion
    }
}
