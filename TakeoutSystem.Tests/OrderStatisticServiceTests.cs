using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TakeoutSystem.Base;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Tests
{
    public class OrderStatisticServiceTests
    {
        [Test]
        public async Task CanceledOrdersCountTest()
        {
            var orderService = new Mock<IOrderService>();
            var orders = new List<OrderDTO>();
            orders.Add(new OrderDTO { ClientName = "CLIENT-001", OrderCode = "ORDER-CODE-001", Status = 0, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-002", OrderCode = "ORDER-CODE-002", Status = 0, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-003", OrderCode = "ORDER-CODE-003", Status = 0, Total = 1, CreatedAt = DateTime.Now });
            orderService.Setup(o => o.GetOrdersAsync(It.IsAny<OrderRequest>())).Returns(Task.FromResult(orders));
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var cancelledOrdersCount = await orderStatistics.CanceledOrdersCountAsync(orderStatisticRequest);

            Assert.That(cancelledOrdersCount == 3);
        }

        [Test]
        public async Task CanceledOrdersPercentageTest()
        {
            var orderService = new Mock<IOrderService>();
            var orders = new List<OrderDTO>();
            orders.Add(new OrderDTO { ClientName = "CLIENT-001", OrderCode = "ORDER-CODE-001", Status = 1, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-002", OrderCode = "ORDER-CODE-002", Status = 0, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-003", OrderCode = "ORDER-CODE-003", Status = 1, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-004", OrderCode = "ORDER-CODE-004", Status = 1, Total = 1, CreatedAt = DateTime.Now });
            orderService.Setup(o => o.GetOrdersAsync(It.IsAny<OrderRequest>())).Returns(Task.FromResult(orders));
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var canceledOrdersPercentage = await orderStatistics.CanceledOrdersPercentageAsync(orderStatisticRequest);

            Assert.That(canceledOrdersPercentage == 25.0m);
        }

        [Test]
        public async Task AverageItemsPerOrderTest()
        {
            var orderService = new Mock<IOrderService>();
            var orders = new List<OrderDTO>();
            orders.Add(new OrderDTO
            {
                ClientName = "CLIENT-001",
                OrderCode = "ORDER-CODE-001",
                Items = new List<ItemOrderDTO> {
                    new ItemOrderDTO { ItemId = 1, Price = 1.5m, Quantity = 6 },
                    new ItemOrderDTO { ItemId = 2, Price = 2.5m, Quantity = 2 },
                    new ItemOrderDTO { ItemId = 3, Price = 1.0m, Quantity = 1 }
                }
            });
            orders.Add(new OrderDTO
            {
                ClientName = "CLIENT-002",
                OrderCode = "ORDER-CODE-002",
                Items = new List<ItemOrderDTO> {
                    new ItemOrderDTO { ItemId = 2, Price = 2.5m, Quantity = 2 },
                    new ItemOrderDTO { ItemId = 3, Price = 1.0m, Quantity = 1 }
                }
            });
            orders.Add(new OrderDTO
            {
                ClientName = "CLIENT-003",
                OrderCode = "ORDER-CODE-003",
                Items = new List<ItemOrderDTO> {
                    new ItemOrderDTO { ItemId = 1, Price = 1.5m, Quantity = 3 }
                }
            });
            orderService.Setup(o => o.GetOrdersAsync(It.IsAny<OrderRequest>())).Returns(Task.FromResult(orders));
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var averageItemsPerOrder = await orderStatistics.AverageItemsPerOrderAsync(orderStatisticRequest);

            Assert.That(averageItemsPerOrder == 5m);
        }

        [Test]
        public async Task AverageServeTimeTest()
        {
            var orderService = new Mock<IOrderService>();
            var orders = new List<OrderDTO>();
            orders.Add(new OrderDTO { ClientName = "CLIENT-001", OrderCode = "ORDER-CODE-001", Status = 1, Total = 1, CreatedAt = Convert.ToDateTime("2021-12-01 12:00:30"), ServedAt = Convert.ToDateTime("2021-12-01 12:05:39") });
            orders.Add(new OrderDTO { ClientName = "CLIENT-002", OrderCode = "ORDER-CODE-002", Status = 1, Total = 1, CreatedAt = Convert.ToDateTime("2021-12-01 12:31:20"), ServedAt = Convert.ToDateTime("2021-12-01 12:39:00") });
            orders.Add(new OrderDTO { ClientName = "CLIENT-003", OrderCode = "ORDER-CODE-003", Status = 1, Total = 1, CreatedAt = Convert.ToDateTime("2021-12-02 16:25:00"), ServedAt = Convert.ToDateTime("2021-12-02 16:30:05") });
            orders.Add(new OrderDTO { ClientName = "CLIENT-004", OrderCode = "ORDER-CODE-004", Status = 1, Total = 1, CreatedAt = Convert.ToDateTime("2021-12-02 17:57:00"), ServedAt = Convert.ToDateTime("2021-12-02 18:15:21") });
            orders.Add(new OrderDTO { ClientName = "CLIENT-005", OrderCode = "ORDER-CODE-005", Status = 1, Total = 1, CreatedAt = Convert.ToDateTime("2021-12-03 11:55:03"), ServedAt = Convert.ToDateTime("2021-12-03 12:05:43") });
            orderService.Setup(o => o.GetOrdersAsync(It.IsAny<OrderRequest>())).Returns(Task.FromResult(orders));
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var averageServeTime = await orderStatistics.AverageServeTimeAsync(orderStatisticRequest);

            Assert.That(averageServeTime == 563m);
        }

        [Test]
        public async Task TotalCountTest()
        {
            var orderService = new Mock<IOrderService>();
            var orders = new List<OrderDTO>();
            orders.Add(new OrderDTO { ClientName = "CLIENT-001", OrderCode = "ORDER-CODE-001", Status = 1, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-002", OrderCode = "ORDER-CODE-002", Status = 1, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-003", OrderCode = "ORDER-CODE-003", Status = 0, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-004", OrderCode = "ORDER-CODE-004", Status = 0, Total = 1, CreatedAt = DateTime.Now });
            orderService.Setup(o => o.GetOrdersAsync(It.IsAny<OrderRequest>())).Returns(Task.FromResult(orders));
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var totalCount = await orderStatistics.TotalCountAsync(orderStatisticRequest);

            Assert.That(totalCount == 4);
        }

        [Test]
        public async Task MostSoldItemsTest()
        {
            var orderService = new Mock<IOrderService>();
            var orderItems = new List<ItemOrderDTO>();
            orderItems.Add(new ItemOrderDTO { ItemId = 1, Name = "ITEM-001", Price = 1.49m, Quantity = 3, Total = 3 * 1.49m });
            orderItems.Add(new ItemOrderDTO { ItemId = 1, Name = "ITEM-001", Price = 1.49m, Quantity = 2, Total = 1 * 1.49m });
            orderItems.Add(new ItemOrderDTO { ItemId = 1, Name = "ITEM-001", Price = 1.49m, Quantity = 2, Total = 2 * 1.49m });
            orderItems.Add(new ItemOrderDTO { ItemId = 2, Name = "ITEM-002", Price = 3.10m, Quantity = 5, Total = 5 * 3.10m });
            orderItems.Add(new ItemOrderDTO { ItemId = 2, Name = "ITEM-002", Price = 3.10m, Quantity = 1, Total = 1 * 3.10m });
            orderItems.Add(new ItemOrderDTO { ItemId = 3, Name = "ITEM-003", Price = 2.50m, Quantity = 1, Total = 1 * 2.50m });
            orderItems.Add(new ItemOrderDTO { ItemId = 3, Name = "ITEM-003", Price = 2.50m, Quantity = 4, Total = 4 * 2.50m });
            orderItems.Add(new ItemOrderDTO { ItemId = 3, Name = "ITEM-003", Price = 2.50m, Quantity = 3, Total = 1 * 2.50m });
            orderService.Setup(o => o.GetOrderItemsAsync(null)).Returns(Task.FromResult(orderItems));
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var mostSoldItems = await orderStatistics.MostSoldItemsAsync(orderStatisticRequest);

            Assert.That(mostSoldItems.Count() == 2);
            Assert.That(mostSoldItems[0].ItemId == 3);
            Assert.That(mostSoldItems[1].ItemId == 1);
            Assert.That(mostSoldItems[0].Name == "ITEM-003");
            Assert.That(mostSoldItems[1].Name == "ITEM-001");
        }

        [Test]
        public async Task TotalPriceOrdersTest()
        {
            var orderService = new Mock<IOrderService>();
            var orders = new List<OrderDTO>();
            orders.Add(new OrderDTO
            {
                ClientName = "CLIENT-001",
                OrderCode = "ORDER-CODE-001",
                Items = new List<ItemOrderDTO> {
                    new ItemOrderDTO { ItemId = 1, Price = 1.5m, Quantity = 6 },
                    new ItemOrderDTO { ItemId = 2, Price = 2.5m, Quantity = 2 },
                    new ItemOrderDTO { ItemId = 3, Price = 1.0m, Quantity = 1 }
                }
            });
            orders.Add(new OrderDTO
            {
                ClientName = "CLIENT-002",
                OrderCode = "ORDER-CODE-002",
                Items = new List<ItemOrderDTO> {
                    new ItemOrderDTO { ItemId = 2, Price = 2.5m, Quantity = 2 },
                    new ItemOrderDTO { ItemId = 3, Price = 1.0m, Quantity = 1 }
                }
            });
            orders.Add(new OrderDTO
            {
                ClientName = "CLIENT-003",
                OrderCode = "ORDER-CODE-003",
                Items = new List<ItemOrderDTO> {
                    new ItemOrderDTO { ItemId = 1, Price = 1.5m, Quantity = 3 }
                }
            });
            orderService.Setup(o => o.GetOrdersAsync(It.IsAny<OrderRequest>())).Returns(Task.FromResult(orders));
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var totalPriceOrders = await orderStatistics.TotalPriceOrdersAsync(orderStatisticRequest);

            Assert.That(totalPriceOrders == 25.5m);
        }

        [Test]
        public async Task AveragePriceOrdersTest()
        {
            var orderService = new Mock<IOrderService>();
            var orders = new List<OrderDTO>();
            orders.Add(new OrderDTO
            {
                ClientName = "CLIENT-001",
                OrderCode = "ORDER-CODE-001",
                Items = new List<ItemOrderDTO> {
                    new ItemOrderDTO { ItemId = 1, Price = 1.5m, Quantity = 6 },
                    new ItemOrderDTO { ItemId = 2, Price = 2.5m, Quantity = 2 },
                    new ItemOrderDTO { ItemId = 3, Price = 1.0m, Quantity = 1 }
                }
            });
            orders.Add(new OrderDTO
            {
                ClientName = "CLIENT-002",
                OrderCode = "ORDER-CODE-002",
                Items = new List<ItemOrderDTO> {
                    new ItemOrderDTO { ItemId = 2, Price = 2.5m, Quantity = 2 },
                    new ItemOrderDTO { ItemId = 3, Price = 1.0m, Quantity = 1 }
                }
            });
            orders.Add(new OrderDTO
            {
                ClientName = "CLIENT-003",
                OrderCode = "ORDER-CODE-003",
                Items = new List<ItemOrderDTO> {
                    new ItemOrderDTO { ItemId = 1, Price = 1.5m, Quantity = 3 }
                }
            });
            orderService.Setup(o => o.GetOrdersAsync(It.IsAny<OrderRequest>())).Returns(Task.FromResult(orders));
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var averagePriceOrders = await orderStatistics.AveragePriceOrdersAsync(orderStatisticRequest);

            Assert.That(averagePriceOrders == 8.5m);
        }
    }
}
