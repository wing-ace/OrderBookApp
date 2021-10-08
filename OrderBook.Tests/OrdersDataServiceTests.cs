using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using OrderBook.BLL.OrdersData.Services.Interfaces;
using OrderBook.CommonTools.Extensions;
using OrderBook.DataContracts.Orders.Enums;
using OrderBook.DataContracts.Orders.Models;
using OrderBook.Tests.Helpers;
using Xunit;

namespace OrderBook.Tests
{
    /// <summary>
    /// Tests for <see cref="IOrdersDataService"/>
    /// </summary>
    public class OrdersDataServiceTests : IClassFixture<CommonTestsFixture>
    {
        private readonly ServiceProvider _serviceProvider;

        public OrdersDataServiceTests(CommonTestsFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        /// <summary>
        /// Scenario:
        ///     1) Filling internal lists of fake fetch background service with values from technical test document
        ///     2) Calling the method of <see cref="IOrdersDataService"/> for get orders data
        ///     3) Checking correct count of orders on each side
        ///     4) Checking correct calculation of cummulative volume for each order
        /// </summary>
        [Fact]
        public void CheckCorrectCalculationOfCummulativeVolume()
        {
            SetNewOrdersDataForFakeBackgroundService(OrderTypeEnum.BuyOrder, new List<OrderDataFromExternalApiDto>
            {
                CreateOrderDataFromExternalApiDto(100, 1),
                CreateOrderDataFromExternalApiDto(99, 2)
            });

            SetNewOrdersDataForFakeBackgroundService(OrderTypeEnum.SellOrder, new List<OrderDataFromExternalApiDto>
            {
                CreateOrderDataFromExternalApiDto(107, 4),
                CreateOrderDataFromExternalApiDto(110, 1)
            });

            var ordersDataService = _serviceProvider.GetService<IOrdersDataService>();
            Assert.NotNull(ordersDataService);

            var buyOrdersList = ordersDataService.GetOrdersSummaryDataByType(OrderTypeEnum.BuyOrder, 0, 1, 10).OrdersList;
            var sellOrdersList = ordersDataService.GetOrdersSummaryDataByType(OrderTypeEnum.SellOrder, 0, 1, 10).OrdersList;

            Assert.Equal(2, buyOrdersList.Count);
            Assert.Equal(100, buyOrdersList[0].CumulativeVolume);
            Assert.Equal(298, buyOrdersList[1].CumulativeVolume);

            Assert.Equal(2, sellOrdersList.Count);
            Assert.Equal(428, sellOrdersList[0].CumulativeVolume);
            Assert.Equal(538, sellOrdersList[1].CumulativeVolume);
        }

        /// <summary>
        /// Scenario
        ///     1) Filling internal lists of fake fetch background service with values from technical test document
        ///     2) Calling the method of <see cref="IOrdersDataService"/> for get orders data
        ///     3) Check that has only 2 orders from each side because only they fit a depth of 550  
        /// </summary>
        [Fact]
        public void CheckCorrectWorkOfDepthFilter()
        {
            SetNewOrdersDataForFakeBackgroundService(OrderTypeEnum.BuyOrder, new List<OrderDataFromExternalApiDto>
            {
                CreateOrderDataFromExternalApiDto(100, 1),
                CreateOrderDataFromExternalApiDto(99, 2),
                CreateOrderDataFromExternalApiDto(98, 3)
            });

            SetNewOrdersDataForFakeBackgroundService(OrderTypeEnum.SellOrder, new List<OrderDataFromExternalApiDto>
            {
                CreateOrderDataFromExternalApiDto(107, 4),
                CreateOrderDataFromExternalApiDto(110, 1),
                CreateOrderDataFromExternalApiDto(113, 2)
            });

            var ordersDataService = _serviceProvider.GetService<IOrdersDataService>();
            Assert.NotNull(ordersDataService);

            var depthValue = 550M;

            var buyOrdersList = ordersDataService.GetOrdersSummaryDataByType(OrderTypeEnum.BuyOrder, depthValue, 1, 10).OrdersList;
            var sellOrdersList = ordersDataService.GetOrdersSummaryDataByType(OrderTypeEnum.SellOrder, depthValue, 1, 10).OrdersList;

            Assert.Equal(buyOrdersList.Count, sellOrdersList.Count);
        }

        /// <summary>
        /// Set the test orders data to internal list of orders in <see cref="FakeFetchOrdersDataBackgroundService"/>
        /// </summary>
        /// <param name="orderType"><see cref="OrderTypeEnum"/></param>
        /// <param name="ordersList">The set of orders</param>
        private void SetNewOrdersDataForFakeBackgroundService(OrderTypeEnum orderType, List<OrderDataFromExternalApiDto> ordersList)
        {
            var backgroundService = GetFakeFetchOrdersDataBackgroundService();
            backgroundService.PrepareTestData(orderType, ordersList);
        }

        /// <summary>
        /// Get the fake background service from test <see cref="ServiceCollection"/>
        /// </summary>
        /// <returns>The fake background service</returns>
        private FakeFetchOrdersDataBackgroundService GetFakeFetchOrdersDataBackgroundService()
        {
            var backgroundService = _serviceProvider.GetHostedService<FakeFetchOrdersDataBackgroundService>();
            Assert.NotNull(backgroundService);

            return backgroundService;
        }

        /// <summary>
        /// Create an instance of <see cref="OrderDataFromExternalApiDto"/>
        /// </summary>
        /// <param name="price">The order price</param>
        /// <param name="volume">The current volume of asset/currency</param>
        /// <returns>The instance of <see cref="OrderDataFromExternalApiDto"/></returns>
        private static OrderDataFromExternalApiDto CreateOrderDataFromExternalApiDto(decimal price, decimal volume)
            => new OrderDataFromExternalApiDto
            {
                Volume = volume,
                Price = price
            };
    }
}
