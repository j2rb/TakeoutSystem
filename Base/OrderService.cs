using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderService : IOrderService
    {
        private readonly TodoContext _context;

        public OrderService(TodoContext context)
        {
            _context = context;
        }

        public List<OrderDetailDTO> GetOrders(OrderRequest orderRequest)
        {
            orderRequest.page = orderRequest.page == null || orderRequest.page <= 0 ? 1 : orderRequest.page;
            orderRequest.pageSize = orderRequest.pageSize == null || orderRequest.pageSize <= 0 ? 10 : orderRequest.pageSize;
            orderRequest.onlyPending = orderRequest.onlyPending == null ? true : orderRequest.onlyPending;
            var query = _context.Orders
                .Where(o => o.Status == 1);
            
            if (orderRequest.onlyPending.GetValueOrDefault() == true)
            {
                query = query.Where(o => o.ServedAt == null);
            }
            var orders = query.Select(o => new OrderDetailDTO
            {
                OrderCode = o.OrderCode,
                ClientName = o.ClientName
            })
                .Skip(orderRequest.page.GetValueOrDefault() * orderRequest.pageSize.GetValueOrDefault() - orderRequest.pageSize.GetValueOrDefault())
                .Take(orderRequest.pageSize.GetValueOrDefault())
                .ToList();
            for (var i = 0; i < orders.Count; i++)
            {
                var orderItems = GetOrderItems(orders[i].OrderCode);
                orders[i].Items = orderItems;
                orders[i].Total = orders[i].Items.Count();
            }
            return orders;
        }

        public OrderDetailDTO GetOrder(String orderCode)
        {
            Order order = _context.Orders.SingleOrDefault(o => (
                   o.OrderCode.Equals(orderCode) && o.Status == 1
               ));
            if (order != null)
            {
                var orderItems = GetOrderItems(orderCode);
                return _context.Orders
                    .Where(o => o.OrderCode.Equals(orderCode) && o.Status == 1)
                    .Select(o => new OrderDetailDTO
                    {
                        OrderCode = o.OrderCode,
                        ClientName = o.ClientName,
                        Total = orderItems.Count(),
                        Items = orderItems
                    })
                    .ToList().First();
            }
            else
            {
                return null;
            }
        }

        public List<ItemOrderDTO> GetOrderItems(String orderCode)
        {
            return _context.OrderItems
                .Join(
                    _context.Items, oi => oi.ItemId, i => i.ItemId, (orderItem, item) => new { orderItem, item }
                )
                .Join(
                    _context.Orders, oi => oi.orderItem.OrderId, o => o.OrderId, (orderItem, order) => new { orderItem.orderItem, orderItem.item, order }
                )
                .Where(oi => oi.order.OrderCode.Equals(orderCode))
                .Select(oi => new ItemOrderDTO
                {
                    ItemId = oi.item.ItemId,
                    Name = oi.item.Name,
                    Price = oi.item.Price,
                    Quantity = oi.orderItem.Quantity,
                    Total = (oi.orderItem.Quantity * oi.item.Price)
                }).ToList();
        }

        public OrderDetailDTO Create(OrderCreationRequest orderCreationRequest)
        {
            if (String.IsNullOrEmpty(orderCreationRequest.ClientName))
            {
                throw new ArgumentException();
            }
            if (orderCreationRequest.Items == null || orderCreationRequest.Items.Count == 0)
            {
                throw new ArgumentException();
            }
            else
            {
                IEnumerable<int> duplicates = orderCreationRequest.Items.GroupBy(i => i.ItemId).Where(i => i.Count() > 1).Select(i => i.Key);
                if (duplicates.Count() > 0)
                {
                    throw new ArgumentException();
                }
                for (var x = 0; x < orderCreationRequest.Items.Count; x++)
                {
                    if (orderCreationRequest.Items[x].ItemId <= 0)
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        Item item = _context.Items.SingleOrDefault(i => i.ItemId == orderCreationRequest.Items[x].ItemId);
                        if (item == null)
                        {
                            throw new ArgumentException();
                        }
                    }
                    if (orderCreationRequest.Items[x].Quantity <= 0)
                    {
                        throw new ArgumentException();
                    }
                }
            }
            IOrderCodeGenerator orderCodeGenerator = new OrderCodeGenerator();
            var order = new Order
            {
                OrderCode = orderCodeGenerator.GetCode(),
                ClientName = orderCreationRequest.ClientName,
                CreatedAt = DateTime.Now,
                Status = 1
            };
            _context.Orders.Add(order);
            try
            {
                _context.SaveChanges();
                for (var i = 0; i < orderCreationRequest.Items.Count; i++)
                {
                    _context.OrderItems.Add(new OrderItem
                    {
                        OrderId = order.OrderId,
                        ItemId = orderCreationRequest.Items[i].ItemId,
                        Quantity = orderCreationRequest.Items[i].Quantity
                    });
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException();
            }
            return GetOrder(order.OrderCode);
        }

        public OrderDetailDTO Cancel(OrderActionRequest orderActionRequest)
        {
            var order = _context.Orders.SingleOrDefault(o => (
                o.OrderCode.Equals(orderActionRequest.OrderCode) && o.ServedAt == null && o.Status == 1
            ));
            if (order != null)
            {
                order.Status = 0;
                _context.Entry(order).State = EntityState.Modified;
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new DbUpdateConcurrencyException();
                }
                return GetOrder(orderActionRequest.OrderCode);
            }
            else
            {
                return null;
            }
        }

        public OrderDetailDTO Serve(OrderActionRequest orderCreationRequest)
        {
            var order = _context.Orders.SingleOrDefault(o => (
                   o.OrderCode.Equals(orderCreationRequest.OrderCode) && o.ServedAt == null && o.Status == 1
               ));
            if (order != null)
            {
                var result = GetOrder(orderCreationRequest.OrderCode);
                order.ServedAt = DateTime.Now;
                _context.Entry(order).State = EntityState.Modified;
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new DbUpdateConcurrencyException();
                }
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
