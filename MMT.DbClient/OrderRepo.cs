using Dapper;
using Microsoft.Extensions.Configuration;
using MMT.DbClient.Interfaces;
using MMT.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MMT.DbClient
{
    public class OrderRepo : IOrderRepo
    {
        private string _connectionString;

        public OrderRepo(IConfigurationRoot config)
        {
            _connectionString = config["ConnectionStrings:OrdersDb"];
        }

        public async Task<Order> GetLatestByCustomerId(string customerId)
        {
            const string selectQuery = @"
DECLARE @OrderId int
SELECT TOP 1 @OrderId = OrderId FROM Orders WHERE CustomerId = @CustomerId ORDER BY OrderDate DESC
SELECT TOP 1 * FROM Orders WHERE OrderId = @OrderId
SELECT * FROM OrderItems WHERE OrderId = @OrderId
SELECT pr.* FROM Products pr JOIN OrderItems oi ON oi.ProductId = pr.ProductId WHERE oi.OrderId = @OrderId
";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                using (SqlMapper.GridReader resultSets = await db.QueryMultipleAsync(selectQuery, new { CustomerId = customerId }))
                {
                    Order order = resultSets.Read<Order>().First();
                    List<OrderItem> orderItems = resultSets.Read<OrderItem>().ToList();
                    List<Product> products = resultSets.Read<Product>().ToList();

                    foreach (OrderItem item in orderItems)
                    {
                        item.Product = products.Where(p => p.ProductId == item.ProductId).FirstOrDefault();
                    }

                    order.Items = orderItems.ToArray();

                    return order;
                }
            }
        }
    }
}
