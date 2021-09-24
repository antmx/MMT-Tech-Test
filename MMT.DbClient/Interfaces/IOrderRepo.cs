using MMT.Models;
using System;
using System.Threading.Tasks;

namespace MMT.DbClient.Interfaces
{
    public interface IOrderRepo
    {
        public Task<Order> GetLatestByCustomerId(string customerId);
    }
}
