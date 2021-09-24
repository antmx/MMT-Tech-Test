using Microsoft.Extensions.Configuration;
using MMT.CustomerApiClient.Interfaces;
using MMT.Models;
using System;
using System.Threading.Tasks;

namespace MMT.CustomerAccountDetailsApiClient
{
    public class CustomerProvider : ICustomerProvider
    {
        private IHttpClientWrapper _httpClientWrapper;
        IConfigurationRoot _config;

        public CustomerProvider(IHttpClientWrapper httpClientWrapper, IConfigurationRoot config)
        {
            _httpClientWrapper = httpClientWrapper;
            _config = config;
        }

        public async Task<Customer> GetByEmailAddress(string emailAddress)
        {
            var client = new ApiClient<Customer>(_config["CustomerDetailsApiBaseUrl"], _config["CustomerDetailsApiKey"], _httpClientWrapper);

            return await client.GetItem("GetUserDetails", new { email = emailAddress });
        }
    }
}
