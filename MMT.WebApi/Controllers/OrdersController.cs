using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MMT.CustomerApiClient.Interfaces;
using MMT.DbClient.Interfaces;
using MMT.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MMT.WebApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderRepo _orderRepo;
        private readonly ICustomerProvider _customerProvider;

        public OrdersController(ILogger<OrdersController> logger, IOrderRepo orderRepo, ICustomerProvider customerProvider)
        {
            _logger = logger;
            _orderRepo = orderRepo;
            _customerProvider = customerProvider;
        }

        [HttpGet]
        public string Index()
        {
            return "Hello, World!";
        }

        [HttpPost]
        [Route("FetchCustomerOrderLatestByEmailAddress")]
        public async Task<object> FetchCustomerOrderLatestByEmailAddress(string emailAddress)
        {
            try
            {
                object info = await GetInfo(emailAddress);

                if (info != null)
                {
                    return new JsonResult(info);
                }

                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred", ex);
                throw;
            }
        }

        private async Task<object> GetInfo(string emailAddress)
        {
            Customer customer = await _customerProvider.GetByEmailAddress(emailAddress);

            if (customer != null)
            {
                Order order = await _orderRepo.GetLatestByCustomerId(customer.CustomerId);

                if (order == null)
                {
                    order = new Order();
                }

                if (order != null)
                {
                    // Return object with both customer and order info
                    return new
                    {
                        customer = new { firstName = customer.FirstName, lastName = customer.LastName },
                        order = new
                        {
                            orderNumber = order.OrderId,
                            orderDate = order.OrderDate,
                            deliveryAddress = customer.Address,
                            orderItems = order.Items.Select(i => new { product = i.Product?.ProductName, quantity = i.Quantity, priceEach = i.Price }).ToArray()
                        },
                        deliveryExpected = order.DeliveryExpected
                    };
                }
                else
                {
                    // Return object with just customer info
                    return new
                    {
                        customer = new { firstName = customer.FirstName, lastName = customer.LastName }
                    };
                }
            }

            _logger.LogInformation($"Customer not found for email address {emailAddress}");

            return null;
        }
    }
}
