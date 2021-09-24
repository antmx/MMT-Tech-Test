using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MMT.CustomerAccountDetailsApiClient
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        public async Task<string> GetAsync(string requestUri)
        {
            using (var httpClient = new HttpClient())
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(requestUri))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    return apiResponse;
                }
            }
        }
    }
}
