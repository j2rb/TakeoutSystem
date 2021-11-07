using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TakeoutSystem.Base;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Tests
{
    public class OrderServiceTests
    {
        private TodoContext _context;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase("TakeoutSystem").Options;
            _context = new TodoContext(options);
            //ITEMS
            _context.Items.Add(new Item
            {
                ItemId = 100,
                Name = "ITEM-100",
                Price = 2.30m
            });
            _context.Items.Add(new Item
            {
                ItemId = 101,
                Name = "ITEM-101",
                Price = 1.79m
            });
            _context.Items.Add(new Item
            {
                ItemId = 102,
                Name = "ITEM-102",
                Price = 3.45m
            });
            _context.SaveChanges();
            //ORDERS
            _context.Orders.Add(new Order
            {
                OrderId = 1,
                ClientName = "ClIENT-1001",
                OrderCode = "ORDER-CODE-001",
                CreatedAt = Convert.ToDateTime("2021-10-05 18:05:23"),
                ServedAt = null,
                Status = 1
            });
            _context.Orders.Add(new Order
            {
                OrderId = 2,
                ClientName = "ClIENT-1002",
                OrderCode = "ORDER-CODE-002",
                CreatedAt = Convert.ToDateTime("2021-10-06 11:55:09"),
                ServedAt = null,
                Status = 1
            });
            _context.Orders.Add(new Order
            {
                OrderId = 3,
                ClientName = "ClIENT-1003",
                OrderCode = "ORDER-CODE-003",
                CreatedAt = DateTime.Now,
                ServedAt = DateTime.Now,
                Status = 1
            });
            _context.Orders.Add(new Order
            {
                OrderId = 4,
                ClientName = "ClIENT-1004",
                OrderCode = "ORDER-CODE-004",
                CreatedAt = DateTime.Now,
                ServedAt = DateTime.Now,
                Status = 1
            });
            _context.SaveChanges();
            //ORDER-ITEMS
            _context.OrderItems.Add(new OrderItem
            {
                OrderId = 1,
                ItemId = 100,
                Quantity = 1
            });
            _context.OrderItems.Add(new OrderItem
            {
                OrderId = 1,
                ItemId = 101,
                Quantity = 2
            });
            _context.OrderItems.Add(new OrderItem
            {
                OrderId = 2,
                ItemId = 100,
                Quantity = 2
            });
            _context.OrderItems.Add(new OrderItem
            {
                OrderId = 2,
                ItemId = 102,
                Quantity = 3
            });
            _context.OrderItems.Add(new OrderItem
            {
                OrderId = 3,
                ItemId = 100,
                Quantity = 1
            });
            _context.OrderItems.Add(new OrderItem
            {
                OrderId = 4,
                ItemId = 102,
                Quantity = 4
            });
            _context.SaveChanges();
        }

        [Test]
        public async Task CreateOrderTest()
        {
            var orderCreationRequest = new OrderCreationRequest
            {
                ClientName = "CLIENT-001",
                Items = new List<OrderItemCreationRequest>()
            };
            orderCreationRequest.Items.Add(new OrderItemCreationRequest { ItemId = 102, Quantity = 1 });
            orderCreationRequest.Items.Add(new OrderItemCreationRequest { ItemId = 101, Quantity = 2 });
            orderCreationRequest.Items.Add(new OrderItemCreationRequest { ItemId = 100, Quantity = 3 });

            IOrderService orderService = new OrderService(_context);
            var order = await orderService.Create(orderCreationRequest);

            Assert.That(order.ClientName == orderCreationRequest.ClientName);
            Assert.That(String.IsNullOrEmpty(order.OrderCode) == false);
            Assert.That(order.ServedAt == null);
            Assert.That(order.Status == 1);
            Assert.That(order.Items.Count() == orderCreationRequest.Items.Count());
            for (var x = 0; x < orderCreationRequest.Items.Count; x++)
            {
                Assert.That(order.Items.Where(i => i.ItemId == orderCreationRequest.Items[x].ItemId).Count() == 1);
                Assert.That(order.Items.Where(i => i.ItemId == orderCreationRequest.Items[x].ItemId).Sum(i => i.Quantity) == orderCreationRequest.Items[x].Quantity);
            }
        }

        [Test]
        public async Task GetOrderTest()
        {
            var clientName = "ClIENT-1001";
            var orderCode = "ORDER-CODE-001";
            var items = 2;

            IOrderService orderService = new OrderService(_context);
            var order = await orderService.GetOrder(orderCode);

            Assert.That(order.OrderCode == orderCode);
            Assert.That(order.ClientName == clientName);
            Assert.That(order.Items.Count() == items);
        }

        [Test]
        public async Task GetAllOrdersTest()
        {
            var orderActionRequest = new OrderRequest
            {
                OnlyPending = false,
                Status = 1
            };

            IOrderService orderService = new OrderService(_context);
            var orders = await orderService.GetOrders(orderActionRequest);

            for (var x = 0; x < orders.Count(); x++)
            {
                Assert.That(orders[x].Status == 1);
            }
        }

        [Test]
        public async Task GetPendingOrdersTest()
        {
            var orderActionRequest = new OrderRequest
            {
                OnlyPending = true,
                Status = 1
            };

            IOrderService orderService = new OrderService(_context);
            var orders = await orderService.GetOrders(orderActionRequest);

            for (var x = 0; x < orders.Count(); x++)
            {
                Assert.That(orders[x].Status == 1);
                Assert.That(orders[x].ServedAt == null);
            }
        }

        [Test]
        public async Task GetOrdersByDatesTest()
        {
            var orderActionRequest = new OrderRequest
            {
                OnlyPending = false,
                StartDate = Convert.ToDateTime("2021-10-01 00:00:00"),
                EndDate = Convert.ToDateTime("2021-10-06 23:59:59")
            };

            IOrderService orderService = new OrderService(_context);
            var orders = await orderService.GetOrders(orderActionRequest);

            for (var x = 0; x < orders.Count(); x++)
            {
                Assert.That(orders[x].CreatedAt >= orderActionRequest.StartDate);
                Assert.That(orders[x].CreatedAt <= orderActionRequest.EndDate);
            }
        }

        [Test]
        public async Task GetOrdersPaginationTest()
        {
            var orderActionRequest = new OrderRequest
            {
                OnlyPending = false,
                Page = 2,
                PageSize = 2
            };

            IOrderService orderService = new OrderService(_context);
            var orders = await orderService.GetOrders(orderActionRequest);

            Assert.That(orders.Count() == 2);
        }

        [Test]
        public async Task GetServedOrdersTest()
        {
            var orderActionRequest = new OrderRequest
            {
                Served = true
            };

            IOrderService orderService = new OrderService(_context);
            var orders = await orderService.GetOrders(orderActionRequest);

            for (var x = 0; x < orders.Count(); x++)
            {
                Assert.That(orders[x].ServedAt != null);
            }
        }

        [Test]
        public async Task ServeOrderTest()
        {
            var orderActionRequest = new OrderActionRequest {
                OrderCode = "ORDER-CODE-002"
            };

            IOrderService orderService = new OrderService(_context);
            var order = await orderService.Serve(orderActionRequest);

            Assert.That(order.OrderCode == orderActionRequest.OrderCode);
            Assert.That(order.ServedAt != null);
            Assert.That(order.Status == 1);
        }

        [Test]
        public async Task CancelOrderTest()
        {
            var orderActionRequest = new OrderActionRequest
            {
                OrderCode = "ORDER-CODE-001"
            };

            IOrderService orderService = new OrderService(_context);
            var order = await orderService.Cancel(orderActionRequest);

            Assert.That(order.OrderCode == orderActionRequest.OrderCode);
            Assert.That(order.ServedAt == null);
            Assert.That(order.Status == 0);
        }
    }
}
