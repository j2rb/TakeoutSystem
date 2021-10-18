using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
        public void CanceledOrdersCountTest()
        {
            var orderService = new Mock<IOrderService>();
            var orders = new List<OrderDTO>();
            orders.Add(new OrderDTO { ClientName = "CLIENT-001", OrderCode = "ORDER-CODE-001", Status = 0, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-002", OrderCode = "ORDER-CODE-002", Status = 0, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-003", OrderCode = "ORDER-CODE-003", Status = 0, Total = 1, CreatedAt = DateTime.Now });
            orderService.Setup(o => o.GetOrders(It.IsAny<OrderRequest>())).Returns(orders);
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var cancelledOrdersCount = orderStatistics.CanceledOrdersCount(orderStatisticRequest);

            Assert.That(cancelledOrdersCount == 3);
        }

        [Test]
        public void CanceledOrdersPercentageTest()
        {
            var orderService = new Mock<IOrderService>();
            var orders = new List<OrderDTO>();
            orders.Add(new OrderDTO { ClientName = "CLIENT-001", OrderCode = "ORDER-CODE-001", Status = 1, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-002", OrderCode = "ORDER-CODE-002", Status = 0, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-003", OrderCode = "ORDER-CODE-003", Status = 1, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-004", OrderCode = "ORDER-CODE-004", Status = 1, Total = 1, CreatedAt = DateTime.Now });
            orderService.Setup(o => o.GetOrders(It.IsAny<OrderRequest>())).Returns(orders);
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var canceledOrdersPercentage = orderStatistics.CanceledOrdersPercentage(orderStatisticRequest);

            Assert.That(canceledOrdersPercentage == 25.0m);
        }

        [Test]
        public void AverageItemsPerOrderTest()
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
            orderService.Setup(o => o.GetOrders(It.IsAny<OrderRequest>())).Returns(orders);
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var averageItemsPerOrder = orderStatistics.AverageItemsPerOrder(orderStatisticRequest);

            Assert.That(averageItemsPerOrder == 5m);
        }

        [Test]
        public void AverageServeTimeTest()
        {
            var orderService = new Mock<IOrderService>();
            var orders = new List<OrderDTO>();
            orders.Add(new OrderDTO { ClientName = "CLIENT-001", OrderCode = "ORDER-CODE-001", Status = 1, Total = 1, CreatedAt = Convert.ToDateTime("2021-12-01 12:00:30"), ServedAt = Convert.ToDateTime("2021-12-01 12:05:39") });
            orders.Add(new OrderDTO { ClientName = "CLIENT-002", OrderCode = "ORDER-CODE-002", Status = 1, Total = 1, CreatedAt = Convert.ToDateTime("2021-12-01 12:31:20"), ServedAt = Convert.ToDateTime("2021-12-01 12:39:00") });
            orders.Add(new OrderDTO { ClientName = "CLIENT-003", OrderCode = "ORDER-CODE-003", Status = 1, Total = 1, CreatedAt = Convert.ToDateTime("2021-12-02 16:25:00"), ServedAt = Convert.ToDateTime("2021-12-02 16:30:05") });
            orders.Add(new OrderDTO { ClientName = "CLIENT-004", OrderCode = "ORDER-CODE-004", Status = 1, Total = 1, CreatedAt = Convert.ToDateTime("2021-12-02 17:57:00"), ServedAt = Convert.ToDateTime("2021-12-02 18:15:21") });
            orders.Add(new OrderDTO { ClientName = "CLIENT-005", OrderCode = "ORDER-CODE-005", Status = 1, Total = 1, CreatedAt = Convert.ToDateTime("2021-12-03 11:55:03"), ServedAt = Convert.ToDateTime("2021-12-03 12:05:43") });
            orderService.Setup(o => o.GetOrders(It.IsAny<OrderRequest>())).Returns(orders);
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var averageServeTime = orderStatistics.AverageServeTime(orderStatisticRequest);

            Assert.That(averageServeTime == 563m);
        }

        [Test]
        public void TotalCountTest()
        {
            var orderService = new Mock<IOrderService>();
            var orders = new List<OrderDTO>();
            orders.Add(new OrderDTO { ClientName = "CLIENT-001", OrderCode = "ORDER-CODE-001", Status = 1, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-002", OrderCode = "ORDER-CODE-002", Status = 1, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-003", OrderCode = "ORDER-CODE-003", Status = 0, Total = 1, CreatedAt = DateTime.Now });
            orders.Add(new OrderDTO { ClientName = "CLIENT-004", OrderCode = "ORDER-CODE-004", Status = 0, Total = 1, CreatedAt = DateTime.Now });
            orderService.Setup(o => o.GetOrders(It.IsAny<OrderRequest>())).Returns(orders);
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var totalCount = orderStatistics.TotalCount(orderStatisticRequest);

            Assert.That(totalCount == 4);
        }

        [Test]
        public void MostSoldItemsTest()
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
            orderService.Setup(o => o.GetOrderItems(null)).Returns(orderItems);
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var mostSoldItems = orderStatistics.MostSoldItems(orderStatisticRequest);

            Assert.That(mostSoldItems.Count() == 2);
            Assert.That(mostSoldItems[0].ItemId == 3);
            Assert.That(mostSoldItems[1].ItemId == 1);
            Assert.That(mostSoldItems[0].Name == "ITEM-003");
            Assert.That(mostSoldItems[1].Name == "ITEM-001");
        }

        [Test]
        public void TotalPriceOrdersTest()
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
            orderService.Setup(o => o.GetOrders(It.IsAny<OrderRequest>())).Returns(orders);
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var totalPriceOrders = orderStatistics.TotalPriceOrders(orderStatisticRequest);

            Assert.That(totalPriceOrders == 25.5m);
        }

        [Test]
        public void AveragePriceOrdersTest()
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
            orderService.Setup(o => o.GetOrders(It.IsAny<OrderRequest>())).Returns(orders);
            var orderStatisticRequest = new OrderStatisticRequest();

            IOrderStatistics orderStatistics = new OrderStatisticts(orderService.Object);
            var averagePriceOrders = orderStatistics.AveragePriceOrders(orderStatisticRequest);

            Assert.That(averagePriceOrders == 8.5m);
        }
    }
}
