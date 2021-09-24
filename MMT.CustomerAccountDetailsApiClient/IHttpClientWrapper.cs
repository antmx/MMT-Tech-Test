using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MMT.CustomerAccountDetailsApiClient
{
    public interface IHttpClientWrapper
    {
        Task<string> GetAsync(string requestUri);
    }
}
