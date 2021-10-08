using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderBook.BLL.OrdersData.Managers;
using OrderBook.BLL.OrdersData.Services.Interfaces;
using OrderBook.DataContracts.Orders.Models;
using OrderBook.ExceptionsHandler.Interfaces;
using OrderBookWebApp.Controllers;
using Xunit;

namespace OrderBook.Tests
{
    /// <summary>
    /// The set of tests for check work of web app controllers
    /// </summary>
    public class ControllersTests
    {
        /// <summary>
        /// Scenario:
        ///     1) Trying to create the home controller and checking it is not null
        ///     2) Trying to get main page view and check it is type of <see cref="ViewResult"/>
        /// </summary>
        [Fact]
        public void SmokeTestForCheckReceivingMainPageViewFromHomeController()
        {
            var homeController = new HomeController();
            Assert.NotNull(homeController);

            var mainPageView = homeController.Index();
            Assert.IsType<ViewResult>(mainPageView);
        }

        /// <summary>
        /// Scenario:
        ///     1) Trying to create the order book controller and checking it is not null
        ///     2) Creating a mock for <see cref="OrdersDataManager"/> and trying to get some data from controller
        ///     3) Checking that it has some data
        /// </summary>
        [Fact]
        public void SmokeTestForGetSomeDataFromOrderBookController()
        {
            var mockOfOrdersDataService = new Mock<IOrdersDataService>();
            var mockOfExceptionsHandler = new Mock<IExceptionsHandler>();

            mockOfOrdersDataService.Setup(service => service.GetBuyOrdersSummaryData(100, 1, 10))
                .Returns(() => new SummaryOrdersDataInfoDto());
            var mockOfOrdersDataManager = new OrdersDataManager(mockOfOrdersDataService.Object, mockOfExceptionsHandler.Object);

            var orderBookController = new OrderBookController(mockOfOrdersDataManager);
            Assert.NotNull(orderBookController);

            var buyOrdersData = orderBookController.GetBuyOrdersData(100, 1, 10);
            Assert.NotNull(buyOrdersData);
            Assert.IsType<JsonResult>(buyOrdersData);

            var jsonResult = (JsonResult)buyOrdersData;
            Assert.NotNull(jsonResult.Value);
        }
    }
}
