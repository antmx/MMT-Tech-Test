using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMT.CustomerAccountDetailsApiClient
{
    public class ApiClient<TModel> where TModel : new()
    {
        private string _apiBaseUrl;
        private string _apiKey;
        private IHttpClientWrapper _httpClientWrapper;

        public ApiClient(string apiBaseUrl, string apiKey, IHttpClientWrapper httpClientWrapper)
        {
            _apiBaseUrl = apiBaseUrl;
            _apiKey = apiKey;
            _httpClientWrapper = httpClientWrapper;
        }

        public async Task<TModel> GetItem(string apiFunctionName, object requestArgs)
        {
            TModel model = default(TModel);

            Dictionary<string, object> requestArgDict = requestArgs.GetType().GetProperties().ToDictionary(pi => pi.Name, pi => pi.GetValue(requestArgs, null));

            requestArgDict.Add("code", _apiKey);

            string requestArgQuery = string.Join("&", requestArgDict.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            string response = await _httpClientWrapper.GetAsync($"{_apiBaseUrl.TrimEnd('/')}/{apiFunctionName.TrimStart('/')}?{requestArgQuery}");

            model = JsonConvert.DeserializeObject<TModel>(response);

            return model;
        }
    }
}
