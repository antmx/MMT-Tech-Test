using MMT.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace MMT.CustomerAccountDetailsApiClient.Tests
{
    [TestFixture]
    public class ApiClientTests
    {
        [Test]
        public async Task GetItem_Returns_Single_ItemAsync()
        {
            var mockHttp = new Mock<IHttpClientWrapper>();

            mockHttp.Setup(hc => hc.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(
                @"{
""email"": ""bob@mmtdigital.co.uk"",
    ""customerId"": ""R223232"",
    ""website"": true,
    ""firstName"": ""Bob"",
    ""lastName"": ""Testperson"",
    ""lastLoggedIn"": ""03-May-2021 09:15"",
    ""houseNumber"": ""1a"",
    ""street"": ""Uppingham Gate"",
    ""town"": ""Uppingham"",
    ""postcode"": ""LE15 9NY"",
    ""preferredLanguage"": ""en-gb""
}"));

            var sut = new ApiClient<Customer>("https://FAKE-customer-account-details.azurewebsites.net/api/", Guid.NewGuid().ToString(), mockHttp.Object);

            Customer customer = await sut.GetItem("GetUserDetails", new { email = "bob@mmtdigital.co.uk" });

            mockHttp.Verify(hcw => hcw.GetAsync(It.IsAny<string>()), Times.Once());

            Assert.AreEqual(customer.CustomerId, "R223232");
        }
    }
}
