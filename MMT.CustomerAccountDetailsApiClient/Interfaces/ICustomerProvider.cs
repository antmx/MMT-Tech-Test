using MMT.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace MMT.CustomerApiClient.Interfaces
{
    public interface ICustomerProvider
    {
        Task<Customer> GetByEmailAddress(string emailAddress);
    }
}
